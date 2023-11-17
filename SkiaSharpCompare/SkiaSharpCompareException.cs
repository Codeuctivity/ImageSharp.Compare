using System;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// SkiaSharpCompareException gets thrown if comparing of images fails
    /// </summary>
    public class SkiaSharpCompareException : Exception
    {
        /// <summary>
        /// SkiaSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        public SkiaSharpCompareException()
        {
        }

        /// <summary>
        /// SkiaSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public SkiaSharpCompareException(string message) : base(message)
        {
        }

        /// <summary>
        /// SkiaSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public SkiaSharpCompareException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}