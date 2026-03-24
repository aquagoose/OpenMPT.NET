using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenMPT.NET;

/// <summary>
/// Provides the native libopenmpt functions.
/// </summary>
public static unsafe class MptNative
{
    public const string LibName = "libopenmpt";

    public const string CTL_Load_SkipSamples = "load.skip_samples";
    public const string CTL_Load_SkipPatterns = "load.skip_patterns";
    public const string CTL_Load_SkipPlugins = "load.skip_plugins";
    public const string CTL_Load_SkipSubsongsInit = "load.skip_subsongs_init";
    public const string CTL_Seek_SyncSamples = "seek.sync_samples";
    public const string CTL_Subsong = "subsong";
    public const string CTL_Play_AtEnd = "play.at_end";
    public const string CTL_Play_TempoFactor = "play.tempo_factor";
    public const string CTL_Play_PitchFactor = "play.pitch_factor";
    public const string CTL_Render_Resampler_EmulateAmiga = "render.resampler.emulate_amiga";
    public const string CTL_Render_Resampler_EmulateAmigaType = "render.resampler.emulate_amiga_type";
    public const string CTL_Render_OPL_VolumeFactor = "render.opl.volume_factor";
    public const string CTL_Dither = "dither";

    [DllImport(LibName, EntryPoint = "openmpt_module_create_from_memory2")]
    public static extern IntPtr ModuleCreateFromMemory(void* data, nuint fileSize,
        delegate*<sbyte*, void*, void> logFunc, void* logUser, delegate*<int, void*, int> errorFunc, void* errorUser,
        int* error, sbyte** errorMessage, Ctl* clts);

    [DllImport(LibName, EntryPoint = "openmpt_module_destroy")]
    public static extern void ModuleDestroy(IntPtr module);

    [DllImport(LibName, EntryPoint = "openmpt_module_read_interleaved_float_stereo")]
    public static extern nuint ModuleReadInterleavedFloatStereo(IntPtr mod, int sampleRate, nuint count, float* interleavedStereo);
    
    [DllImport(LibName, EntryPoint = "openmpt_module_read_interleaved_stereo")]
    public static extern nuint ModuleReadInterleavedStereo(IntPtr mod, int sampleRate, nuint count, short* interleavedStereo);

    [DllImport(LibName, EntryPoint = "openmpt_module_set_position_order_row")]
    public static extern double ModuleSetPositionOrderRow(IntPtr mod, int order, int row);

    [DllImport(LibName, EntryPoint = "openmpt_module_set_position_seconds")]
    public static extern int ModuleSetPositionSeconds(IntPtr mod, double seconds);

    [DllImport(LibName, EntryPoint = "openmpt_module_get_position_seconds")]
    public static extern double ModuleGetPositionSeconds(IntPtr mod);
    
    [DllImport(LibName, EntryPoint = "openmpt_module_set_render_param")]
    public static extern int ModuleSetRenderParam(IntPtr mod, int parameter, int value);

    [DllImport(LibName, EntryPoint = "openmpt_module_get_render_param")]
    public static extern int ModuleGetRenderParam(IntPtr mod, int parameter, int* value);

    [DllImport(LibName, EntryPoint = "openmpt_module_get_duration_seconds")]
    public static extern double ModuleGetDurationSeconds(IntPtr mod);

    [DllImport(LibName, EntryPoint = "openmpt_module_get_metadata_keys")]
    public static extern sbyte* ModuleGetMetadataKeys(IntPtr mod);
    
    [DllImport(LibName, EntryPoint = "openmpt_module_get_metadata")]
    public static extern sbyte* ModuleGetMetadata(IntPtr mod, sbyte* key);

    [DllImport(LibName, EntryPoint = "openmpt_module_ctl_get")]
    public static extern string ModuleCTLGet(IntPtr module, string ctl);

    [DllImport(LibName, EntryPoint = "openmpt_module_ctl_set")]
    public static extern int ModuleCTLSet(IntPtr module, string ctl, string value);

    public unsafe struct Ctl// : IDisposable
    {
        public sbyte* Key;
        public sbyte* Value;

        public Ctl(string key, string value)
        {
            Key = (sbyte*) Marshal.StringToHGlobalAnsi(key);
            Value = (sbyte*) Marshal.StringToHGlobalAnsi(value);
        }
    }
}