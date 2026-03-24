namespace OpenMPT.NET;

public enum AmigaResamplerType
{
    /// <summary>
    /// Filter type is chosen by the library and might change. This is the default.
    /// </summary>
    Auto,
    
    /// <summary>
    /// Amiga A500 filter.
    /// </summary>
    A500,
    
    /// <summary>
    /// Amiga A1200 filter.
    /// </summary>
    A1200,
    
    /// <summary>
    /// BLEP synthesis without model-specific filters. The LED filter is ignored by this setting. This filter mode is
    /// considered to be experimental and might change in the future.
    /// </summary>
    Unfiltered
}