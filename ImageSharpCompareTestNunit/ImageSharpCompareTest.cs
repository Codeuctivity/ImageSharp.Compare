using Codeuctivity.ImageSharpCompare;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

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
        private const string colorShift1 = "../../../TestData/ColorShift1.png";
        private const string colorShift2 = "../../../TestData/ColorShift2.png";

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
        [TestCase(colorShift1, colorShift2, ResizeOption.DontResize, false)]
        public void ShouldVerifyThatImagesWithDifferentSizeAreEqual(string pathActual, string pathExpected, ResizeOption resizeOption, bool expectedResult)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(ImageSharpCompare.ImagesAreEqual(absolutePathActual, absolutePathExpected, resizeOption), Is.EqualTo(expectedResult));
        }
        [Test]
        [TestCase(colorShift1, colorShift2, ResizeOption.DontResize, 0, false)]
        [TestCase(colorShift1, colorShift2, ResizeOption.Resize, 0, false)]
        [TestCase(colorShift1, colorShift2, ResizeOption.DontResize, 15, true)]
        [TestCase(colorShift1, colorShift2, ResizeOption.Resize, 15, true)]
        public void ShouldVerifyThatImagesWithColorShift(string pathActual, string pathExpected, ResizeOption resizeOption, int expectedColorShift, bool expectedResult)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(ImageSharpCompare.ImagesAreEqual(absolutePathActual, absolutePathExpected, resizeOption, expectedColorShift), Is.EqualTo(expectedResult));
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
            AssertDisposeBehavior.AssertThatImageIsDisposed(actual);
            AssertDisposeBehavior.AssertThatImageIsDisposed(expected);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageSharpImagesAreEqualBrga(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = Image.Load(absolutePathActual);
            using var expected = Image.Load<Bgra32>(absolutePathExpected);

            Assert.That(ImageSharpCompare.ImagesAreEqual(actual, expected), Is.True);
            AssertDisposeBehavior.AssertThatImageIsDisposed(actual);
            AssertDisposeBehavior.AssertThatImageIsDisposed(expected);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageSharpImagesAreEqualBgra5551(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = Image.Load<Bgra5551>(absolutePathActual);
            using var expected = Image.Load<Bgra5551>(absolutePathExpected);

            Assert.That(ImageSharpCompare.ImagesAreEqual(actual, expected), Is.True);
            AssertDisposeBehavior.AssertThatImageIsDisposed(actual);
            AssertDisposeBehavior.AssertThatImageIsDisposed(expected);
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, ResizeOption.DontResize, 0)]
        [TestCase(jpg0Rgb24, png0Rgba32, 384538, 2.3789191061839596d, 140855, 87.139021553537404d, ResizeOption.Resize, 0)]
        [TestCase(jpg1Rgb24, png1Rgba32, 382669, 2.3673566603152607d, 140893, 87.162530004206772d, ResizeOption.DontResize, 0)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize, 0)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208832, 1.2919254658385093d, 2089, 1.2923461433768035d, ResizeOption.DontResize, 0)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize, 0)]
        [TestCase(pngBlack2x2px, pngWhite2x2px, 3060, 765, 4, 100.0d, ResizeOption.DontResize, 0)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize, 0)]
        [TestCase(renderedForm1, renderedForm2, 50103469, 61.825603405725566d, 220164, 27.167324777887465d, ResizeOption.Resize, 0)]
        [TestCase(renderedForm2, renderedForm1, 50103469, 61.825603405725566d, 220164, 27.167324777887465d, ResizeOption.Resize, 0)]
        [TestCase(colorShift1, colorShift2, 117896, 3.437201166180758d, 30398, 88.623906705539355d, ResizeOption.DontResize, 0)]
        [TestCase(colorShift1, colorShift2, 0, 0, 0, 0, ResizeOption.DontResize, 15)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int pixelColorShiftTolerance)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var diff = ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, resizeOption, pixelColorShiftTolerance);

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

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize, 0, false)]
        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.Resize, 0, false)]
        [TestCase(pngWhite2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0, false)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0, false)]
        [TestCase(renderedForm1, renderedForm2, 0, 0, 0, 0, ResizeOption.Resize, 0, false)]
        [TestCase(renderedForm2, renderedForm1, 0, 0, 0, 0, ResizeOption.Resize, 0, false)]
        [TestCase(colorShift1, colorShift1, 0, 0, 0, 0, ResizeOption.DontResize, 15, true)]
        [TestCase(colorShift1, colorShift1, 0, 0, 0, 0, ResizeOption.Resize, 15, true)]
        [TestCase(colorShift1, colorShift2, 0, 0, 0, 0, ResizeOption.Resize, 15, true)]
        [TestCase(colorShift1, colorShift2, 0, 0, 0, 0, ResizeOption.DontResize, 15, true)]
        [TestCase(colorShift1, colorShift2, 0, 0, 0, 0, ResizeOption.Resize, 14, false)]
        [TestCase(colorShift1, colorShift2, 0, 0, 0, 0, ResizeOption.DontResize, 14, false)]
        public void CalcDiffMaskImage(string pathPic1, string pathPic2, double expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int expectedColorShift, bool expectMaskToBeBlack)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMask = Path.GetTempFileName() + "differenceMask.png";

            using (var fileStreamDifferenceMask = File.Create(differenceMask))
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(absolutePathPic1, absolutePathPic2, resizeOption, expectedColorShift))
            {
                ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
                Assert.That(IsImageEntirelyBlack(maskImage), Is.EqualTo(expectMaskToBeBlack));
            }

            var maskedDiff = ImageSharpCompare.CalcDiff(absolutePathPic1, absolutePathPic2, differenceMask, resizeOption, expectedColorShift);
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
        public void ShouldCalcDiffMaskImageSharpAndUseOutcome(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
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
            AssertDisposeBehavior.
                        AssertThatImageIsDisposed(absolutePic1);
            AssertDisposeBehavior.AssertThatImageIsDisposed(absolutePic2);
            AssertDisposeBehavior.AssertThatImageIsDisposed(differenceMaskPic);
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        public void ShouldCalcDiffMaskImageHalfSingleHalfVector2AndUseOutcome(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPicPath = Path.GetTempFileName() + "differenceMask.png";

            using var absolutePic1 = Image.Load<HalfVector2>(absolutePathPic1);
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
            AssertDisposeBehavior.
                        AssertThatImageIsDisposed(absolutePic1);
            AssertDisposeBehavior.AssertThatImageIsDisposed(absolutePic2);
            AssertDisposeBehavior.AssertThatImageIsDisposed(differenceMaskPic);
        }

        [TestCase(pngWhite2x2px, pngBlack2x2px, pngTransparent2x2px, 765, 12240, 16, 100d, ResizeOption.Resize, 0)]
        [TestCase(pngWhite2x2px, pngBlack2x2px, pngBlack4x4px, 765, 12240, 16, 100d, ResizeOption.Resize, 0)]
        [TestCase(pngBlack2x2px, pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(pngBlack4x4px, pngBlack2x2px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(colorShift1, colorShift2, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 15)]
        public void ShouldUseDiffMask(string pathPic1, string pathPic2, string pathPic3, double expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int pixelColorShiftTolerance)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = Image.Load(absolutePathPic1);
            using var pic2 = Image.Load(absolutePathPic2);
            using var maskPic = Image.Load(differenceMaskPic);

            var maskedDiff = ImageSharpCompare.CalcDiff(pic1, pic2, maskPic, resizeOption, pixelColorShiftTolerance);

            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
            AssertDisposeBehavior.
                        AssertThatImageIsDisposed(pic1);
            AssertDisposeBehavior.AssertThatImageIsDisposed(pic2);
            AssertDisposeBehavior.AssertThatImageIsDisposed(maskPic);
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
            AssertDisposeBehavior.
                        AssertThatImageIsDisposed(pic1);
            AssertDisposeBehavior.AssertThatImageIsDisposed(pic2);
            AssertDisposeBehavior.AssertThatImageIsDisposed(maskPic);
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
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByFilePath_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using (var diffMask1Stream = File.Create(diffMask1Path))
            {
                using var diffMask1Image = ImageSharpCompare.CalcDiffMaskImage(image1Path, image2Path);
                ImageExtensions.SaveAsPng(diffMask1Image, diffMask1Stream);
            }

            using var diffMask2Image = ImageSharpCompare.CalcDiffMaskImage(image1Path, image2Path, diffMask1Path);
            Assert.That(IsImageEntirelyBlack(diffMask2Image), Is.True);

            File.Delete(diffMask1Path);
        }

        [TestCase(png0Rgba32, png1Rgba32)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByStream_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using var image1Stream = new FileStream(image1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var image2Stream = new FileStream(image2Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (var diffMask1Stream = File.Create(diffMask1Path))
            {
                using var diffMask1Image = ImageSharpCompare.CalcDiffMaskImage(image1Stream, image2Stream);
                ImageExtensions.SaveAsPng(diffMask1Image, diffMask1Stream);
            }

            image1Stream.Position = 0;
            image2Stream.Position = 0;

            using (var diffMask1Stream = new FileStream(diffMask1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                diffMask1Stream.Position = 0;
                using var diffMask2Image = ImageSharpCompare.CalcDiffMaskImage(image1Stream, image2Stream, diffMask1Stream);
                Assert.That(IsImageEntirelyBlack(diffMask2Image), Is.True);
            }

            File.Delete(diffMask1Path);
        }

        [TestCase(png0Rgba32, png1Rgba32)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByImage_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);

            using var image1 = Image.Load(image1Path);
            using var image2 = Image.Load(image2Path);

            using var diffMask1Image = ImageSharpCompare.CalcDiffMaskImage(image1, image2);

            using var diffMask2Image = ImageSharpCompare.CalcDiffMaskImage(image1, image2, diffMask1Image);

            Assert.That(IsImageEntirelyBlack(diffMask2Image), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg1Rgb24)]
        [TestCase(png0Rgba32, png1Rgba32)]
        [TestCase(jpg0Rgb24, png1Rgba32)]
        [TestCase(jpg0Rgb24, png0Rgba32)]
        [TestCase(jpg1Rgb24, png1Rgba32)]
        [TestCase(colorShift1, colorShift2)]
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
        [TestCase(colorShift1, colorShift2)]
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
            if (image is not Image<Rgb24> imageRgb24)
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