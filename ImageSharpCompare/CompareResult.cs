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
        public CompareResult
        (int meanError, int absoluteError)
        {
            MeanError = meanError;
            AbsoluteError = absoluteError;
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
    }
}