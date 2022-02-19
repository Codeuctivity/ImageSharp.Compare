using System;
using System.Runtime.Serialization;

namespace Codeuctivity.ImageSharpCompare
{
    /// <summary>
    /// ImageSharpCompareException gets thrown if comparing of images fails
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// ImageSharpCompareException gets thrown if comparing of images fails
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ImageSharpCompareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}