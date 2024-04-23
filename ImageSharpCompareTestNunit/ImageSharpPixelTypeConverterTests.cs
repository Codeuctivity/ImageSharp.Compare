using Codeuctivity.ImageSharpCompare;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

namespace ImageSharpCompareTestNunit
{
    public class ImageSharpPixelTypeConverterTests
    {
        private const string pngBlack2x2px = "../../../TestData/Black.png";

        [Test]
        public void ShouldConvertToRgb24()
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pngBlack2x2px);

            var image = Image.Load<Rgb24>(absolutePathActual);

            var rgb24 = ImageSharpPixelTypeConverter.ToRgb24Image(image, out var isClonedInstance);

            Assert.That(isClonedInstance, Is.False);
            Assert.That(rgb24.PixelType.BitsPerPixel, Is.EqualTo(24));

            image.Dispose();
            AssertDisposeBehavior.AssertThatImageIsDisposed(rgb24, true);
        }

        [Test]
        public void ShouldConvertAnyPixelTypeToRgb24()
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pngBlack2x2px);

            var image = Image.Load<Rgba64>(absolutePathActual);

            var rgb24 = ImageSharpPixelTypeConverter.ToRgb24Image(image, out var isClonedInstance);

            Assert.That(isClonedInstance, Is.True);
            Assert.That(rgb24.PixelType.BitsPerPixel, Is.EqualTo(24));

            image.Dispose();
            AssertDisposeBehavior.AssertThatImageIsDisposed(rgb24);
        }
    }
}
