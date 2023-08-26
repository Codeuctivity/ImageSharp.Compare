using SkiaSharp;
using System.IO;

namespace SkiaSharpCompareTestNunit
{
    internal class ImageExtensions
    {
        protected ImageExtensions()
        {
        }

        internal static void SaveAsPng(SKBitmap diffMask1Image, FileStream diffMask1Stream)
        {
            var encodedData = diffMask1Image.Encode(SKEncodedImageFormat.Png, 100);
            encodedData.SaveTo(diffMask1Stream);
        }
    }
}