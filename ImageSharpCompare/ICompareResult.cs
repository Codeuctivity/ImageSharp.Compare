namespace Codeuctivity.ImageSharpCompare
{
    /// <summary>
    /// Dto - of compared images
    /// </summary>
    public interface ICompareResult
    {
        /// <summary>
        /// Mean relative pixel error
        /// </summary>
        double MeanError { get; }

        /// <summary>
        /// Absolute pixel error
        /// </summary>
        int AbsoluteError { get; }

        /// <summary>
        /// Number of pixels that differ between images
        /// </summary>
        int PixelErrorCount { get; }

        /// <summary>
        /// Percentage of pixels that differ between images
        /// </summary>
        double PixelErrorPercentage { get; }
    }
}