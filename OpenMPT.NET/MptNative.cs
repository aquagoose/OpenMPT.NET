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

    [DllImport(LibName, EntryPoint = "openmpt_module_create_from_memory2")]
    public static extern IntPtr ModuleCreateFromMemory(void* data, nuint fileSize,
        delegate*<sbyte*, void*, void> logFunc, void* logUser, delegate*<int, void*, int> errorFunc, void* errorUser,
        int* error, sbyte** errorMessage, Ctl* clts);

    [DllImport(LibName, EntryPoint = "openmpt_module_destroy")]
    public static extern void ModuleDestroy(IntPtr module);

    [DllImport(LibName, EntryPoint = "openmpt_module_read_interleaved_float_stereo")]
    public static extern nuint ModuleReadInterleavedFloatStereo(IntPtr mod, int sampleRate, nuint count, float* interleavedStereo);

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

    public unsafe struct Ctl// : IDisposable
    {
        public sbyte* Key;
        public sbyte* Value;

        public Ctl(sbyte* key, sbyte* value)
        {
            Key = key;
            Value = value;
        }

        /*public Ctl(string key, string value)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            Key = (sbyte*) NativeMemory.Alloc((nuint) key.Length);
            fixed (byte* ptr = keyBytes)
                Unsafe.CopyBlock(Key, ptr, (uint) key.Length);
            
            byte[] valueBytes = Encoding.ASCII.GetBytes(value);
            Value = (sbyte*) NativeMemory.Alloc((nuint) value.Length);
            fixed (byte* ptr = valueBytes)
                Unsafe.CopyBlock(Value, ptr, (uint) value.Length);
        }

        public void Dispose()
        {
            NativeMemory.Free(Key);
            NativeMemory.Free(Value);
        }*/
    }
}