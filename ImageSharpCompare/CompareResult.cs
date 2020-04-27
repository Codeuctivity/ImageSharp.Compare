namespace Codeuctivity
{
    /// <summary>
    /// Dto - outcome of compared images
    /// </summary>
    public class CompareResult : ICompareResult
    {
        /// <summary>
        /// ctor for CompareResult
        /// </summary>
        /// <param name="meanError">Mean error</param>
        /// <param name="absoluteError">Absolute error</param>
        /// <param name="pixelErrorCount">Number of pixels that differ between images</param>
        /// <param name="pixelErrorPercentage">Percentage of pixels that differ between images</param>
        public CompareResult
                (int meanError, int absoluteError, int pixelErrorCount, double pixelErrorPercentage)
        {
            MeanError = meanError;
            AbsoluteError = absoluteError;
            PixelErrorCount = pixelErrorCount;
            PixelErrorPercentage = pixelErrorPercentage;
        }

        /// <summary>
        /// Mean pixel error
        /// </summary>
        /// <value>0-1</value>
        public int MeanError { get; }

        /// <summary>
        /// Absolute pixel error
        /// </summary>
        public int AbsoluteError { get; }

        /// <summary>
        /// Number of pixels that differ between images
        /// </summary>
        public int PixelErrorCount { get; }

        /// <summary>
        /// Percentage of pixels that differ between images
        /// </summary>
        public double PixelErrorPercentage { get; }
    }
}