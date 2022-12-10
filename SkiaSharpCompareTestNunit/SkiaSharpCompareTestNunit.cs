using Codeuctivity.SkiaSharpCompare;
using NUnit.Framework;
using SkiaSharp;
using System;
using System.IO;
using System.Reflection;

namespace SkiaSharpCompareTestNunit
{
    public class IntegrationTest
    {
        private const string jpg0Rgb24 = "../../../TestData/Calc0.jpg";
        private const string jpg1Rgb24 = "../../../TestData/Calc1.jpg";
        private const string png0Rgba32 = "../../../TestData/Calc0.png";
        private const string png1Rgba32 = "../../../TestData/Calc1.png";
        private const string pngBlack = "../../../TestData/Black.png";
        private const string pngWhite = "../../../TestData/White.png";

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, png0Rgba32, true)]
        [TestCase(png0Rgba32, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, jpg1Rgb24, true)]
        [TestCase(png0Rgba32, pngBlack, false)]
        public void ShouldVerifyThatImagesFromFilepathSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(SkiaSharpCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, png0Rgba32, true)]
        [TestCase(png0Rgba32, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, jpg1Rgb24, true)]
        [TestCase(png0Rgba32, pngBlack, false)]
        public void ShouldVerifyThatImagesSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = SKBitmap.Decode(absolutePathActual);
            using var expected = SKBitmap.Decode(absolutePathExpected);

            Assert.That(SkiaSharpCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, png0Rgba32, true)]
        [TestCase(png0Rgba32, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, jpg1Rgb24, true)]
        [TestCase(png0Rgba32, pngBlack, false)]
        public void ShouldVerifyThatImageStreamsSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(SkiaSharpCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(SkiaSharpCompare.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageStreamsAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(SkiaSharpCompare.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatSkiaSharpImagesAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = SKBitmap.Decode(absolutePathActual);
            using var expected = SKBitmap.Decode(absolutePathExpected);

            Assert.That(SkiaSharpCompare.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d)]
        [TestCase(jpg1Rgb24, png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208890, 1.2922842790329365d, 2101, 1.2997698646408156d)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d)]
        [TestCase(pngBlack, pngWhite, 3060, 765, 4, 100.0d)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var diff = SkiaSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2);
            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d)]
        [TestCase(jpg1Rgb24, png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208890, 1.2922842790329365d, 2101, 1.2997698646408156d)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d)]
        [TestCase(pngBlack, pngWhite, 3060, 765, 4, 100.0d)]
        public void ShouldVerifyThatImageStreamsAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var diff = SkiaSharpCompare.CalcDiff(pic1, pic2);
            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0)]
        public void ShoulCalcDiffmask(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMask = Path.GetTempFileName() + "differenceMask.png";

            using (var fileStreamDifferenceMask = File.Create(differenceMask))
            using (var maskImage = SkiaSharpCompare.CalcDiffMaskImage(absolutePathPic1, absolutePathPic2))
            {
                SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var maskedDiff = SkiaSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, differenceMask);
            File.Delete(differenceMask);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        private void SaveAsPng(SKBitmap maskImage, FileStream fileStreamDifferenceMask)
        {
            var encodedData = maskImage.Encode(SKEncodedImageFormat.Png, 100);
            encodedData.SaveTo(fileStreamDifferenceMask);
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0)]
        public void ShoulCalcDiffmaskSkiaSharp(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPicPath = Path.GetTempFileName() + "differenceMask.png";

            using var absolutePic1 = SKBitmap.Decode(absolutePathPic1);
            using var absolutePic2 = SKBitmap.Decode(absolutePathPic2);

            using (var fileStreamDifferenceMask = File.Create(differenceMaskPicPath))
            using (var maskImage = SkiaSharpCompare.CalcDiffMaskImage(absolutePic1, absolutePic2))
            {
                SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            using var differenceMaskPic = SKBitmap.Decode(differenceMaskPicPath);
            var maskedDiff = SkiaSharpCompare.CalcDiff(absolutePic1, absolutePic2, differenceMaskPic);
            File.Delete(differenceMaskPicPath);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        private static object? GetInstanceField<T>(T instance, string fieldName)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = typeof(T).GetField(fieldName, bindFlags);
            if (field == null)
            {
                throw new ArgumentNullException(fieldName);
            }

            return field.GetValue(instance);
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0)]
        public void DiffmaskSteams(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using var maskImage = SkiaSharpCompare.CalcDiffMaskImage(pic1, pic2);

            pic1.Position = 0;
            pic2.Position = 0;

            var maskedDiff = SkiaSharpCompare.CalcDiff(pic1, pic2, maskImage);
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg1Rgb24)]
        [TestCase(png0Rgba32, png1Rgba32)]
        [TestCase(jpg0Rgb24, png1Rgba32)]
        [TestCase(jpg0Rgb24, png0Rgba32)]
        [TestCase(jpg1Rgb24, png1Rgba32)]
        public void ShouldVerifyThatImagesAreNotEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(SkiaSharpCompare.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.False);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg1Rgb24)]
        [TestCase(png0Rgba32, png1Rgba32)]
        [TestCase(jpg0Rgb24, png1Rgba32)]
        [TestCase(jpg0Rgb24, png0Rgba32)]
        [TestCase(jpg1Rgb24, png1Rgba32)]
        public void ShouldVerifyThatImageStreamAreNotEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(SkiaSharpCompare.ImagesAreEqual(actual, expected), Is.False);
        }

        [TestCase(png0Rgba32, pngBlack)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => SkiaSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }
    }
}