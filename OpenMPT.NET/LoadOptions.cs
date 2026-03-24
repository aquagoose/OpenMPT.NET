namespace OpenMPT.NET;

/// <summary>
/// Options that affect the module loading process.
/// </summary>
public record struct LoadOptions
{
    /// <summary>
    /// Avoid loading samples into memory.
    /// </summary>
    public bool SkipSamples;

    /// <summary>
    /// Avoid loading patterns into memory.
    /// </summary>
    public bool SkipPatterns;

    /// <summary>
    /// Avoid loading plugins.
    /// </summary>
    public bool SkipPlugins;

    /// <summary>
    /// Avoid pre-initializing sub-songs. Skipping results in faster module loading but slower seeking.
    /// </summary>
    public bool SkipSubsongsInit;

    public LoadOptions(bool skipSamples = false, bool skipPatterns = false, bool skipPlugins = false,
        bool skipSubsongsInit = false)
    {
        SkipSamples = skipSamples;
        SkipPatterns = skipPatterns;
        SkipPlugins = skipPlugins;
        SkipSubsongsInit = skipSubsongsInit;
    }
}