using Codeuctivity.ImageSharpCompare;
using NUnit.Framework;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Reflection;

namespace ImageSharpCompareTestNunit
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

            Assert.That(ImageSharpCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
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

            using var actual = Image.Load(absolutePathActual);
            using var expected = Image.Load(absolutePathExpected);

            Assert.That(ImageSharpCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
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

            Assert.That(ImageSharpCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(ImageSharpCompare.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.True);
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

            Assert.That(ImageSharpCompare.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageSharpImagesAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = Image.Load(absolutePathActual);
            using var expected = Image.Load(absolutePathExpected);

            Assert.That(ImageSharpCompare.ImagesAreEqual(actual, expected), Is.True);
            AssertDisposeBehavior(actual);
            AssertDisposeBehavior(expected);
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d)]
        [TestCase(jpg1Rgb24, png1Rgba32, 382669, 2.3673566603152607d, 140893, 87.162530004206772d)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208832, 1.2919254658385093d, 2089, 1.2923461433768035d)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d)]
        [TestCase(pngBlack, pngWhite, 3060, 765, 4, 100.0d)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var diff = ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2);

            Console.WriteLine($"PixelErrorCount: {diff.PixelErrorCount}");
            Console.WriteLine($"PixelErrorPercentage: {diff.PixelErrorPercentage}");
            Console.WriteLine($"AbsoluteError: {diff.AbsoluteError}");
            Console.WriteLine($"MeanError: {diff.MeanError}");

            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d)]
        [TestCase(jpg1Rgb24, png1Rgba32, 382669, 2.3673566603152607d, 140893, 87.162530004206772d)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208832, 1.2919254658385093d, 2089, 1.2923461433768035d)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d)]
        [TestCase(pngBlack, pngWhite, 3060, 765, 4, 100.0d)]
        public void ShouldVerifyThatImageStreamsAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var diff = ImageSharpCompare.CalcDiff(pic1, pic2);
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
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(absolutePathPic1, absolutePathPic2))
            {
                ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var maskedDiff = ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, differenceMask);
            File.Delete(differenceMask);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0)]
        public void ShoulCalcDiffmaskImageSharp(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPicPath = Path.GetTempFileName() + "differenceMask.png";

            using var absolutePic1 = Image.Load(absolutePathPic1);
            using var absolutePic2 = Image.Load(absolutePathPic2);

            using (var fileStreamDifferenceMask = File.Create(differenceMaskPicPath))
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(absolutePic1, absolutePic2))
            {
                ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            using var differenceMaskPic = Image.Load(differenceMaskPicPath);
            var maskedDiff = ImageSharpCompare.CalcDiff(absolutePic1, absolutePic2, differenceMaskPic);
            File.Delete(differenceMaskPicPath);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");

            AssertDisposeBehavior(absolutePic1);
            AssertDisposeBehavior(absolutePic2);
            AssertDisposeBehavior(differenceMaskPic);
        }

        private void AssertDisposeBehavior(Image image)
        {
            const string imageSharpPrivateFieldNameIsDisposed = "isDisposed";
            var isDisposed = (bool?)GetInstanceField(image, imageSharpPrivateFieldNameIsDisposed);
            Assert.That(isDisposed, Is.False);
            image.Dispose();
            isDisposed = (bool?)GetInstanceField(image, imageSharpPrivateFieldNameIsDisposed);
            Assert.That(isDisposed, Is.True);
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

            using var maskImage = ImageSharpCompare.CalcDiffMaskImage(pic1, pic2);

            pic1.Position = 0;
            pic2.Position = 0;

            var maskedDiff = ImageSharpCompare.CalcDiff(pic1, pic2, maskImage);
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

            Assert.That(ImageSharpCompare.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.False);
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

            Assert.That(ImageSharpCompare.ImagesAreEqual(actual, expected), Is.False);
        }

        [TestCase(png0Rgba32, pngBlack)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<ImageSharpCompareException>(() => ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }
    }
}