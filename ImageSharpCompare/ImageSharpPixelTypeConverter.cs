using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Codeuctivity.ImageSharpCompare
{
    /// <summary>
    /// Provides functionality to convert an ImageSharp image of any pixel type to an ImageSharp image with Rgb24 pixel type.
    /// </summary>
    public static class ImageSharpPixelTypeConverter
    {
        /// <summary>
        /// Converts an Image with any pixel type to Rgb24
        /// </summary>
        /// <param name="image"></param>
        /// <param name="isClonedInstance">Use this to dispose cloned instances</param>
        public static Image<Rgb24> ToRgb24Image(Image image, out bool isClonedInstance)
        {
            ArgumentNullException.ThrowIfNull(image);

            if (image is Image<Rgb24> actualPixelAccessibleImage)
            {
                isClonedInstance = false;
                return actualPixelAccessibleImage;
            }

            isClonedInstance = true;
            return image.CloneAs<Rgb24>();
        }
    }
}