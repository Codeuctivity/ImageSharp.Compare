using System;

namespace Codeuctivity.ImageSharpCompare
{
    /// <summary>
    /// ImageSharpCompareException gets thrown if comparing of images fails
    /// </summary>
    public class ImageSharpCompareException : Exception
    {
        /// <summary>
        /// ImageSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        public ImageSharpCompareException()
        {
        }

        /// <summary>
        /// ImageSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ImageSharpCompareException(string message) : base(message)
        {
        }

        /// <summary>
        /// ImageSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public ImageSharpCompareException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}