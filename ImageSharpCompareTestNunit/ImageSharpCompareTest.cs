using Codeuctivity.ImageSharpCompare;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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
        private const string pngBlack2x2px = "../../../TestData/Black.png";
        private const string pngBlack4x4px = "../../../TestData/BlackDoubleSize.png";
        private const string pngWhite2x2px = "../../../TestData/White.png";
        private const string pngTransparent2x2px = "../../../TestData/pngTransparent2x2px.png";
        private const string renderedForm1 = "../../../TestData/HC007-Test-02-3-OxPt.html1.png";
        private const string renderedForm2 = "../../../TestData/HC007-Test-02-3-OxPt.html2.png";

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, png0Rgba32, true)]
        [TestCase(png0Rgba32, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, jpg1Rgb24, true)]
        [TestCase(png0Rgba32, pngBlack2x2px, false)]
        public void ShouldVerifyThatImagesFromFilePathSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
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
        [TestCase(png0Rgba32, pngBlack2x2px, false)]
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
        [TestCase(png0Rgba32, pngBlack2x2px, false)]
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
        [TestCase(pngBlack2x2px, pngBlack2x2px, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, ResizeOption.DontResize, false)]
        public void ShouldVerifyThatImagesWithDifferentSizeAreEqual(string pathActual, string pathExpected, ResizeOption resizeOption, bool expectedResult)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(ImageSharpCompare.ImagesAreEqual(absolutePathActual, absolutePathExpected, resizeOption), Is.EqualTo(expectedResult));
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
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, null)]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, ResizeOption.Resize)]
        [TestCase(jpg1Rgb24, png1Rgba32, 382669, 2.3673566603152607d, 140893, 87.162530004206772d, ResizeOption.DontResize)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208832, 1.2919254658385093d, 2089, 1.2923461433768035d, ResizeOption.DontResize)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize)]
        [TestCase(pngBlack2x2px, pngWhite2x2px, 3060, 765, 4, 100.0d, ResizeOption.DontResize)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize)]
        [TestCase(renderedForm1, renderedForm2, 50103469, 61.825603405725566d, 220164, 27.167324777887465d, ResizeOption.Resize)]
        [TestCase(renderedForm2, renderedForm1, 50103469, 61.825603405725566d, 220164, 27.167324777887465d, ResizeOption.Resize)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var diff = ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, resizeOption);

            Console.WriteLine($"PixelErrorCount: {diff.PixelErrorCount}");
            Console.WriteLine($"PixelErrorPercentage: {diff.PixelErrorPercentage}");
            Console.WriteLine($"AbsoluteError: {diff.AbsoluteError}");
            Console.WriteLine($"MeanError: {diff.MeanError}");

            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(pngBlack2x2px, pngBlack4x4px)]
        [TestCase(pngBlack4x4px, pngWhite2x2px)]
        public void ShouldVerifyThatCalcDiffThrowsOnDifferentImageSizes(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<ImageSharpCompareException>(
                   () => ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, ResizeOption.DontResize));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, null)]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, ResizeOption.Resize)]
        [TestCase(jpg1Rgb24, png1Rgba32, 382669, 2.3673566603152607d, 140893, 87.162530004206772d, ResizeOption.DontResize)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208832, 1.2919254658385093d, 2089, 1.2923461433768035d, ResizeOption.DontResize)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize)]
        [TestCase(pngBlack2x2px, pngWhite2x2px, 3060, 765, 4, 100.0d, ResizeOption.DontResize)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize)]
        public void ShouldVerifyThatImageStreamsAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var diff = ImageSharpCompare.CalcDiff(pic1, pic2, resizeOption);
            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, null)]
        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngWhite2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(renderedForm1, renderedForm2, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(renderedForm2, renderedForm1, 0, 0, 0, 0, ResizeOption.Resize)]
        public void CalcDiffMaskImage(string pathPic1, string pathPic2, double expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMask = Path.GetTempFileName() + "differenceMask.png";

            using (var fileStreamDifferenceMask = File.Create(differenceMask))
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(absolutePathPic1, absolutePathPic2, resizeOption))
            {
                ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var maskedDiff = ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, differenceMask, resizeOption);
            File.Delete(differenceMask);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        public void ShoulCalcDiffMaskImageSharpAndUseOutcome(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPicPath = Path.GetTempFileName() + "differenceMask.png";

            using var absolutePic1 = Image.Load(absolutePathPic1);
            using var absolutePic2 = Image.Load(absolutePathPic2);

            using (var fileStreamDifferenceMask = File.Create(differenceMaskPicPath))
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(absolutePic1, absolutePic2, resizeOption))
            {
                ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            using var differenceMaskPic = Image.Load(differenceMaskPicPath);
            var maskedDiff = ImageSharpCompare.CalcDiff(absolutePic1, absolutePic2, differenceMaskPic, resizeOption);
            File.Delete(differenceMaskPicPath);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");

            AssertDisposeBehavior(absolutePic1);
            AssertDisposeBehavior(absolutePic2);
            AssertDisposeBehavior(differenceMaskPic);
        }

        [TestCase(pngWhite2x2px, pngBlack2x2px, pngTransparent2x2px, 765, 12240, 16, 100d, ResizeOption.Resize)]
        [TestCase(pngWhite2x2px, pngBlack2x2px, pngBlack4x4px, 765, 12240, 16, 100d, ResizeOption.Resize)]
        [TestCase(pngBlack2x2px, pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack4x4px, pngBlack2x2px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize)]
        public void ShouldUseDiffMask(string pathPic1, string pathPic2, string pathPic3, double expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = Image.Load(absolutePathPic1);
            using var pic2 = Image.Load(absolutePathPic2);
            using var maskPic = Image.Load(differenceMaskPic);

            var maskedDiff = ImageSharpCompare.CalcDiff(pic1, pic2, maskPic, resizeOption);

            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");

            AssertDisposeBehavior(pic1);
            AssertDisposeBehavior(pic2);
            AssertDisposeBehavior(maskPic);
        }

        [TestCase(pngBlack2x2px, pngBlack2x2px, pngBlack4x4px)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, pngBlack2x2px)]
        [TestCase(pngBlack4x4px, pngBlack2x2px, pngBlack2x2px)]
        public void ShouldThrowUsingInvalidImageDimensionsDiffMask(string pathPic1, string pathPic2, string pathPic3)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = Image.Load(absolutePathPic1);
            using var pic2 = Image.Load(absolutePathPic2);
            using var maskPic = Image.Load(differenceMaskPic);

            var exception = Assert.Throws<ImageSharpCompareException>(() => ImageSharpCompare.CalcDiff(pic1, pic2, maskPic, ResizeOption.DontResize));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));

            AssertDisposeBehavior(pic1);
            AssertDisposeBehavior(pic2);
            AssertDisposeBehavior(maskPic);
        }

        private static void AssertDisposeBehavior(Image image)
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
            return field == null ? throw new ArgumentNullException(fieldName) : field.GetValue(instance);
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0)]
        public void DiffMaskSteams(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
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

        [TestCase(png0Rgba32, png1Rgba32)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImages_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using var image1Stream = new FileStream(image1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var image2Stream = new FileStream(image2Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (var fileStreamDiffMask1 = File.Create(diffMask1Path))
            using (var diffMask1Image = ImageSharpCompare.CalcDiffMaskImage(image1Stream, image2Stream))
            {
                ImageExtensions.SaveAsPng(diffMask1Image, fileStreamDiffMask1);
            }

            using var diffMask1Stream = new FileStream(diffMask1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            image1Stream.Position = 0;
            image2Stream.Position = 0;
            diffMask1Stream.Position = 0;

            var diffMask2Image = ImageSharpCompare.CalcDiffMaskImage(image1Stream, image2Stream, diffMask1Stream);

            File.Delete(diffMask1Path);

            Assert.That(IsImageEntirelyBlack(diffMask2Image), Is.True);
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

        [TestCase(png0Rgba32, pngBlack2x2px)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<ImageSharpCompareException>(() => ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [TestCase(png0Rgba32, png0Rgba32, pngBlack2x2px)]
        [TestCase(png0Rgba32, pngBlack2x2px, png0Rgba32)]
        [TestCase(pngBlack2x2px, png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2, string pathPic3)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var absolutePathPic3 = Path.Combine(AppContext.BaseDirectory, pathPic3);

            var exception = Assert.Throws<ImageSharpCompareException>(() => ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, absolutePathPic3));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        private static bool IsImageEntirelyBlack(Image image)
        {
            if (!(image is Image<Rgb24> imageRgb24))
            {
                throw new ArgumentException("Image must be an RGB 24 one", nameof(image));
            }

            for (var x = 0; x < imageRgb24.Width; x++)
            {
                for (var y = 0; y < imageRgb24.Height; y++)
                {
                    if (imageRgb24[x, y] != new Rgb24(byte.MinValue, byte.MinValue, byte.MinValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}