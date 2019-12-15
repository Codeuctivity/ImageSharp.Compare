using Codeuctivity;
using NUnit.Framework;
using System.IO;

namespace ImageSharpCompareTestNunit
{
    public class IntegrationTest
    {
        private const string jpg0 = "./TestData/Calc0.jpg";
        private const string jpg1 = "./TestData/Calc1.jpg";
        private const string png0 = "./TestData/Calc0.png";
        private const string png1 = "./TestData/Calc1.png";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(jpg0, jpg0)]
        [TestCase(png0, png0)]
        public void ShouldVerfiyThatImagesAreEqual(string pathActual, string pathExpected)
        {
            Assert.That(ImageSharpCompare.ImageAreEqual(pathActual, pathExpected), Is.True);
        }

        [Test]
        [TestCase(jpg0, png0, 384538, 2)]
        [TestCase(jpg1, png1, 382669, 2)]
        [TestCase(png1, png1, 0, 0)]
        [TestCase(jpg1, jpg1, 0, 0)]
        [TestCase(jpg0, jpg1, 208832, 1)]
        [TestCase(png0, png1, 203027, 1)]
        public void ShouldVerfiyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError)
        {
            var diff = ImageSharpCompare.CalcDiff(pathPic1, pathPic2);
            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
        }

        [TestCase(png0, png1, 0, 0)]
        public void Diffmask(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError)
        {
            using (var fileStreamDifferenceMask = File.Create("differenceMask.png"))
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(pathPic1, pathPic2))
            {
                SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var maskedDiff = ImageSharpCompare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
        }

        [Test]
        [TestCase(jpg0, jpg1)]
        [TestCase(png0, png1)]
        [TestCase(jpg0, png1)]
        [TestCase(jpg0, png0)]
        [TestCase(jpg1, png1)]
        public void ShouldVerifyThatImagesAreNotEqal(string pathActual, string pathExpected)
        {
            Assert.That(ImageSharpCompare.ImageAreEqual(pathActual, pathExpected), Is.False);
        }
    }
}