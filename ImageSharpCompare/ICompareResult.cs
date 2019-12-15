namespace Codeuctivity
{
    /// <summary>
    /// Dto - of compared images
    /// </summary>
    public interface ICompareResult
    {
        /// <summary>
        /// Mean pixel error
        /// </summary>
        /// <value>0-1</value>
        int MeanError { get; }

        /// <summary>
        /// Absolute pixel error
        /// </summary>
        int AbsoluteError { get; }
    }
}