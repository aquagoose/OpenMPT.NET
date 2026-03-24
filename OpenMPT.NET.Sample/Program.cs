using Silk.NET.SDL;
using Thread = System.Threading.Thread;

namespace OpenMPT.NET.Sample;

public class Program
{
    private const uint SampleRate = 48000;
    
    private static Module _module = null!;
    private static ulong _readSamples;
    
    public static unsafe void Main(string[] args)
    {
        Sdl sdl = Sdl.GetApi();
        if (sdl.Init(Sdl.InitAudio) < 0)
            throw new Exception($"Failed to initialize SDL: {sdl.GetErrorS()}");

        AudioSpec spec = new()
        {
            Freq = (int) SampleRate,
            Samples = 512,
            Channels = 2,
            Format = Sdl.AudioF32,
            Callback = new PfnAudioCallback(AudioCallback)
        };

        uint device = sdl.OpenAudioDevice((byte*) null, 0, &spec, null, 0);
        if (device == 0)
            throw new Exception($"Failed to open audio device: {sdl.GetErrorS()}");
        
        _module = Module.FromMemory(File.ReadAllBytes("ag-sundrv.mptm"), new ModuleOptions(pitchFactor: 1.0f));
        //_module.Params.InterpolationFilter = Filter.Linear;

        ModuleMetadata metadata = _module.Metadata;
        Console.WriteLine($"{metadata.Artist ?? "Unknown Artist"} - {metadata.Title ?? "Unknown Title"}");

        double durationSeconds = _module.DurationInSeconds;
        Console.WriteLine($"{(int) durationSeconds / 60:00}:{(int) durationSeconds % 60:00}");

        sdl.PauseAudioDevice(device, 0);
        
        while (_readSamples < _module.DurationInSeconds * SampleRate)
        {
            Thread.Sleep(1000);
    
            double seconds = _module.PositionInSeconds;
            Console.WriteLine($"{(int) seconds / 60:00}:{(int) seconds % 60:00}");
        }

        sdl.CloseAudioDevice(device);
        _module.Dispose();
        sdl.Quit();
        sdl.Dispose();
    }

    private static unsafe void AudioCallback(void* arg0, byte* arg1, int arg2)
    {
        Span<float> buffer = new Span<float>(arg1, arg2 / 4);
        _readSamples += _module.ReadInterleavedStereo(SampleRate, buffer);
    }
}