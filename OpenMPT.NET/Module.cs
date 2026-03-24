using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenMPT.NET;
using static MptNative;

public unsafe class Module : IDisposable
{
    private IntPtr _module;
    
    /// <summary>
    /// Get the current song position in seconds.
    /// </summary>
    public double PositionInSeconds => ModuleGetPositionSeconds(_module);

    /// <summary>
    /// Get the duration of the song in seconds.
    /// </summary>
    public double DurationInSeconds => ModuleGetDurationSeconds(_module);

    public ModuleMetadata Metadata => ModuleMetadata.FromModule(_module);
    
    public RenderParams Params;

    private Module(IntPtr module)
    {
        _module = module;
        Params = new RenderParams(_module);
    }

    public ulong ReadInterleavedStereo(uint sampleRate, Span<float> buffer)
    {
        ulong read;
        fixed (float* pBuffer = buffer)
            read = ModuleReadInterleavedFloatStereo(_module, (int) sampleRate, (nuint) (buffer.Length / 2), pBuffer);

        return read;
    }

    /// <summary>
    /// Attempt to seek to the given order and row.
    /// </summary>
    /// <param name="order">The order to seek to.</param>
    /// <param name="row">The row within that order to seek to.</param>
    /// <returns>The approximate new song position in seconds.</returns>
    public double Seek(int order, int row)
    {
        return ModuleSetPositionOrderRow(_module, order, row);
    }

    /// <summary>
    /// Attempt to seek to the given number of seconds.
    /// </summary>
    /// <param name="seconds">The seconds to seek to.</param>
    /// <returns>The approximate new song position in seconds.</returns>
    public double Seek(double seconds)
    {
        return ModuleSetPositionSeconds(_module, seconds);
    }

    /// <summary>
    /// Create a <see cref="Module"/> from memory.
    /// </summary>
    /// <param name="memory">The module file.</param>
    /// <returns>The loaded module.</returns>
    /// <exception cref="ModuleLoadException">Thrown if the module fails to load.</exception>
    public static Module FromMemory(byte[] memory, ModuleOptions options)
    {
        IntPtr module;

        Ctl[] ctls =
        [
            new Ctl("play.at_end", options.EndBehavior.ToString().ToLower()),
            new Ctl("play.tempo_factor", options.TempoFactor.ToString(CultureInfo.InvariantCulture)),
            new Ctl("play.pitch_factor", options.PitchFactor.ToString(CultureInfo.InvariantCulture)),
            new Ctl("render.resampler.emulate_amiga", options.EmulateAmigaResampler ? "1" : "0")
        ];

        int error;

        fixed (byte* ptr = memory)
        fixed (Ctl* pCtls = ctls)
            module = ModuleCreateFromMemory(ptr, (nuint) memory.Length, null, null, null, null, &error, null, pCtls);

        ModuleResult result = (ModuleResult) error;
        
        if (result != ModuleResult.Ok)
            throw new ModuleLoadException($"An error occurred: {result} (Error code: {error})");

        return new Module(module);
    }

    /// <summary>
    /// Dispose of this <see cref="Module"/>.
    /// </summary>
    public void Dispose()
    {
        ModuleDestroy(_module);
    }
}