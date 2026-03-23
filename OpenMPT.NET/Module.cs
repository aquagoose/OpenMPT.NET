using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenMPT.NET;
using static MptNative;

public unsafe class Module : IDisposable
{
    private IntPtr _module;

    /// <summary>
    /// The number of channels of this module.
    /// </summary>
    public readonly int Channels;
    
    /// <summary>
    /// The sample rate of this module.
    /// </summary>
    public readonly int SampleRate;
    
    /// <summary>
    /// The floating-point interleaved buffer.
    /// </summary>
    public readonly float[] Buffer;

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

        SampleRate = 48000;
        Channels = 2;
        Buffer = new float[SampleRate];

        Params = new RenderParams(_module);
    }

    /// <summary>
    /// Advance the buffer, returning the number of samples advanced by.
    /// </summary>
    /// <returns>The number of samples advanced by.</returns>
    public int AdvanceBuffer()
    {
        nuint read;

        fixed (float* ptr = Buffer)
            read = ModuleReadInterleavedFloatStereo(_module, SampleRate, (nuint) (Buffer.Length / 2), ptr);

        return (int) read;
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
    public static Module FromMemory(byte[] memory)
    {
        IntPtr module;

        int error;

        fixed (byte* ptr = memory)
            module = ModuleCreateFromMemory(ptr, (nuint) memory.Length, null, null, null, null, &error, null, null);

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