using System;
using System.Runtime.Serialization;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// SkiaSharpCompareException gets thrown if comparing of images fails
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// SkiaSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected SkiaSharpCompareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}