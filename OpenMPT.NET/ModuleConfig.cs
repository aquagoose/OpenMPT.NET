using System;
using System.Globalization;
using static OpenMPT.NET.MptNative;

namespace OpenMPT.NET;

/// <summary>
/// Settable module options.
/// </summary>
public readonly record struct ModuleConfig
{
    private readonly IntPtr _module;

    public bool SyncSamples
    {
        get => ModuleCTLGet(_module, CTL_Seek_SyncSamples) == "1";
        set => ModuleCTLSet(_module, CTL_Seek_SyncSamples, value ? "1" : "0");
    }

    /// <summary>
    /// The <see cref="OpenMPT.NET.EndBehavior"/> that occurs at the end of a song.
    /// </summary>
    public EndBehavior EndBehavior
    {
        get => Enum.Parse<EndBehavior>(ModuleCTLGet(_module, CTL_Play_AtEnd), true);
        set => ModuleCTLSet(_module, CTL_Play_AtEnd, value.ToString().ToLower());
    }

    /// <summary>
    /// The floating point tempo factor. A value of 1.0 means no change.
    /// </summary>
    public float TempoFactor
    {
        get => float.Parse(ModuleCTLGet(_module, CTL_Play_TempoFactor));
        set => ModuleCTLSet(_module, CTL_Play_TempoFactor, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// The floating point pitch factor. A value of 1.0 means no change.
    /// </summary>
    public float PitchFactor
    {
        get => float.Parse(ModuleCTLGet(_module, CTL_Play_PitchFactor));
        set => ModuleCTLSet(_module, CTL_Play_PitchFactor, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Emulate the Amiga resampler for Amiga modules.
    /// </summary>
    public bool EmulateAmigaResampler
    {
        get => ModuleCTLGet(_module, CTL_Render_Resampler_EmulateAmiga) == "1";
        set => ModuleCTLSet(_module, CTL_Render_Resampler_EmulateAmiga, value ? "1" : "0");
    }

    public AmigaResamplerType EmulateAmigaType
    {
        get => Enum.Parse<AmigaResamplerType>(ModuleCTLGet(_module, CTL_Render_Resampler_EmulateAmigaType), true);
        set => ModuleCTLSet(_module, CTL_Render_Resampler_EmulateAmigaType, value.ToString().ToLower());
    }

    public float OPLVolumeFactor
    {
        get => float.Parse(ModuleCTLGet(_module, CTL_Render_OPL_VolumeFactor));
        set => ModuleCTLSet(_module, CTL_Render_OPL_VolumeFactor, value.ToString(CultureInfo.InvariantCulture));
    }

    public DitherMode Dither
    {
        get => (DitherMode) int.Parse(ModuleCTLGet(_module, CTL_Dither));
        set => ModuleCTLSet(_module, CTL_Dither, ((int) value).ToString());
    }

    public ModuleConfig(IntPtr module)
    {
        _module = module;
    }
}