using Codeuctivity;
using NUnit.Framework;
using System.IO;

namespace ImageSharpCompareTestNunit
{
    public class IntegrationTest
    {
        private const string jpg0 = "../../../TestData/Calc0.jpg";
        private const string jpg1 = "../../../TestData/Calc1.jpg";
        private const string png0 = "../../../TestData/Calc0.png";
        private const string png1 = "../../../TestData/Calc1.png";
        private const string pngBlack = "../../../TestData/Black.png";
        private const string pngWhite = "../../../TestData/White.png";

        [Test]
        [TestCase(jpg0, jpg0)]
        [TestCase(png0, png0)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected)
        {
            Assert.That(ImageSharpCompare.ImageAreEqual(pathActual, pathExpected), Is.True);
        }

        [Test]
        [TestCase(jpg0, png0, 384538, 2.3789191061839596d, 140855, 87.139021553537404d)]
        [TestCase(jpg1, png1, 382669, 2.3673566603152607d, 140893, 87.162530004206772d)]
        [TestCase(png1, png1, 0, 0, 0, 0)]
        [TestCase(jpg1, jpg1, 0, 0, 0, 0)]
        [TestCase(jpg0, jpg1, 208832, 1.2919254658385093d, 2089, 1.2923461433768035d)]
        [TestCase(png0, png1, 203027, 1.25601321422385d, 681, 0.42129618173269651d)]
        [TestCase(pngBlack, pngWhite, 3060, 765, 4, 100.0d)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var diff = ImageSharpCompare.CalcDiff(pathPic1, pathPic2);
            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0, png1, 0, 0, 0, 0)]
        public void Diffmask(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            using (var fileStreamDifferenceMask = File.Create("differenceMask.png"))
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(pathPic1, pathPic2))
            {
                SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var maskedDiff = ImageSharpCompare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [Test]
        [TestCase(jpg0, jpg1)]
        [TestCase(png0, png1)]
        [TestCase(jpg0, png1)]
        [TestCase(jpg0, png0)]
        [TestCase(jpg1, png1)]
        public void ShouldVerifyThatImagesAreNotEqual(string pathActual, string pathExpected)
        {
            Assert.That(ImageSharpCompare.ImageAreEqual(pathActual, pathExpected), Is.False);
        }
    }
}