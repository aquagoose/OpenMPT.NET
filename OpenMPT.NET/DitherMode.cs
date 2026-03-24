namespace OpenMPT.NET;

public enum DitherMode
{
    /// <summary>
    /// No dithering.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Default mode. Chosen by OpenMPT code, might change.
    /// </summary>
    Default = 1,
    
    /// <summary>
    /// Rectangular, 0.5 bit depth, no noise shaping (original ModPlug Tracker).
    /// </summary>
    Rectangular05 = 2,
    
    /// <summary>
    /// Rectangular, 1 bit depth, simple 1st order noise shaping 
    /// </summary>
    Rectangular10 = 3
}