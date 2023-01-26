using Codeuctivity.SkiaSharpCompare.SkiaSharpCompare;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// Dto - outcome of compared images
    /// </summary>
    public class CompareResult : ICompareResult
    {
        /// <summary>
        /// Mean pixel error of absolute pixel error
        /// </summary>
        /// <value>0-765</value>
        public double MeanError { get; }

        /// <summary>
        /// Absolute pixel error, counts each color channel on every pixel the delta
        /// </summary>
        public int AbsoluteError { get; }

        /// <summary>
        /// Number of pixels that differ between images
        /// </summary>
        public int PixelErrorCount { get; }

        /// <summary>
        /// Percentage of pixels that differ between images
        /// </summary>
        /// <value>0-100.0</value>
        public double PixelErrorPercentage { get; }

        /// <summary>
        /// ctor for CompareResult
        /// </summary>
        /// <param name="meanError">Mean error</param>
        /// <param name="absoluteError">Absolute error</param>
        /// <param name="pixelErrorCount">Number of pixels that differ between images</param>
        /// <param name="pixelErrorPercentage">Percentage of pixels that differ between images</param>
        public CompareResult(int absoluteError, double meanError, int pixelErrorCount, double pixelErrorPercentage)
        {
            MeanError = meanError;
            AbsoluteError = absoluteError;
            PixelErrorCount = pixelErrorCount;
            PixelErrorPercentage = pixelErrorPercentage;
        }
    }
}