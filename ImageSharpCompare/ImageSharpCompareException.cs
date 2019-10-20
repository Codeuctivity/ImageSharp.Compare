using System;
using System.Runtime.Serialization;

namespace Codeuctivity
{
    [Serializable]
    internal class ImageSharpCompareException : Exception
    {
        public ImageSharpCompareException()
        {
        }

        public ImageSharpCompareException(string message) : base(message)
        {
        }

        public ImageSharpCompareException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ImageSharpCompareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}