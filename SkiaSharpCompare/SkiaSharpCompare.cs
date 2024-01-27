using SkiaSharp;
using System;
using System.IO;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// SkiaSharpCompare, compares images. An alpha channel is ignored.
    /// </summary>
    public static class Compare
    {
        private const string sizeDiffersExceptionMessage = "Size of images differ.";

        /// <summary>
        /// Is true if width and height of both images are equal
        /// </summary>
        /// <param name="pathImageActual"></param>
        /// <param name="pathImageExpected"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(string pathImageActual, string pathImageExpected)
        {
            using var actualImage = SKBitmap.Decode(pathImageActual);
            using var expectedImage = SKBitmap.Decode(pathImageExpected);
            return ImagesHaveEqualSize(actualImage, expectedImage);
        }

        /// <summary>
        /// Is true if width and height of both images are equal
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(Stream actual, Stream expected)
        {
            using var actualImage = SKBitmap.Decode(actual);
            using var expectedImage = SKBitmap.Decode(expected);
            return ImagesHaveEqualSize(actualImage, expectedImage);
        }

        /// <summary>
        /// Is true if width and height of both images are equal
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(SKBitmap actualImage, SKBitmap expectedImage)
        {
            return ImagesHaveSameDimension(actualImage, expectedImage);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="pathImageActual"></param>
        /// <param name="pathImageExpected"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(string pathImageActual, string pathImageExpected, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            using var actualImage = SKBitmap.Decode(pathImageActual);
            using var expectedImage = SKBitmap.Decode(pathImageExpected);
            return ImagesAreEqual(actualImage, expectedImage, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(Stream actual, Stream expected, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            using var actualImage = SKBitmap.Decode(actual);
            using var expectedImage = SKBitmap.Decode(expected);
            return ImagesAreEqual(actualImage, expectedImage, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(SKBitmap actual, SKBitmap expected, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            ArgumentNullException.ThrowIfNull(actual);
            ArgumentNullException.ThrowIfNull(expected);

            if (resizeOption == ResizeOption.DontResize && !ImagesHaveSameDimension(actual, expected))
            {
                return false;
            }

            if (resizeOption == ResizeOption.DontResize || ImagesHaveSameDimension(actual, expected))
            {
                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        if (!actual.GetPixel(x, y).Equals(expected.GetPixel(x, y)))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            var grown = GrowToSameDimension(actual, expected);
            try
            {
                return ImagesAreEqual(grown.Item1, grown.Item2, ResizeOption.DontResize);
            }
            finally
            {
                grown.Item1?.Dispose();
                grown.Item2?.Dispose();
            }
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            return CalcDiff(actual, expected, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Stream actualImage, Stream expectedImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            using var actual = SKBitmap.Decode(actualImage);
            using var expected = SKBitmap.Decode(expectedImage);
            return CalcDiff(actual, expected, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(SKBitmap actual, SKBitmap expected, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            var imagesHaveSameDimension = ImagesHaveSameDimension(actual, expected);

            if (resizeOption == ResizeOption.Resize && !imagesHaveSameDimension)
            {
                var grown = GrowToSameDimension(actual, expected);
                try
                {
                    return CalcDiff(grown.Item1, grown.Item2, ResizeOption.DontResize);
                }
                finally
                {
                    grown.Item1?.Dispose();
                    grown.Item2?.Dispose();
                }
            }

            if (!ImagesHaveSameDimension(actual, expected))
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            var quantity = actual.Width * actual.Height;
            var absoluteError = 0;
            var pixelErrorCount = 0;

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    var actualPixel = actual.GetPixel(x, y);
                    var expectedPixel = expected.GetPixel(x, y);

                    var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                    var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                    var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);
                    var sum = r + g + b;
                    absoluteError = absoluteError + (sum > pixelColorShiftTolerance ? sum : 0);
                    pixelErrorCount += (sum > pixelColorShiftTolerance) ? 1 : 0;
                }
            }

            var meanError = (double)absoluteError / quantity;
            var pixelErrorPercentage = (double)pixelErrorCount / quantity * 100;
            return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images using a mask image for tolerated difference between the two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="pathMaskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, string pathMaskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            using var mask = SKBitmap.Decode(pathMaskImage);
            return CalcDiff(actual, expected, mask, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <param name="maskImage"></param>
        /// <returns></returns>
        public static ICompareResult CalcDiff(Stream actualImage, Stream expectedImage, SKBitmap maskImage)
        {
            using var actual = SKBitmap.Decode(actualImage);
            using var expected = SKBitmap.Decode(expectedImage);
            return CalcDiff(actual, expected, maskImage);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images using a image mask for tolerated difference between the two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="maskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(SKBitmap actual, SKBitmap expected, SKBitmap maskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            ArgumentNullException.ThrowIfNull(maskImage);

            var imagesHaveSameDimension = ImagesHaveSameDimension(actual, expected) && ImagesHaveSameDimension(actual, maskImage);

            if (resizeOption == ResizeOption.Resize && !imagesHaveSameDimension)
            {
                var grown = GrowToSameDimension(actual, expected, maskImage);
                try
                {
                    return CalcDiff(grown.Item1, grown.Item2, grown.Item3, ResizeOption.DontResize, pixelColorShiftTolerance);
                }
                finally
                {
                    grown.Item1?.Dispose();
                    grown.Item2?.Dispose();
                    grown.Item3?.Dispose();
                }
            }

            if (!imagesHaveSameDimension)
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            var quantity = actual.Width * actual.Height;
            var absoluteError = 0;
            var pixelErrorCount = 0;

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    var maskImagePixel = maskImage.GetPixel(x, y);
                    var actualPixel = actual.GetPixel(x, y);
                    var expectedPixel = expected.GetPixel(x, y);

                    var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                    var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                    var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);

                    var error = 0;

                    if (r > maskImagePixel.Red)
                    {
                        error += r;
                    }

                    if (g > maskImagePixel.Green)
                    {
                        error += g;
                    }

                    if (b > maskImagePixel.Blue)
                    {
                        error += b;
                    }

                    absoluteError = absoluteError + (error > pixelColorShiftTolerance ? error : 0);
                    pixelErrorCount += error > pixelColorShiftTolerance ? 1 : 0;
                }
            }
            var meanError = (double)absoluteError / quantity;
            var pixelErrorPercentage = (double)pixelErrorCount / quantity * 100;
            return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage);
        }

        private static bool ImagesHaveSameDimension(SKBitmap actual, SKBitmap expected)
        {
            ArgumentNullException.ThrowIfNull(actual);

            ArgumentNullException.ThrowIfNull(expected);

            return actual.Height == expected.Height && actual.Width == expected.Width;
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(string pathActualImage, string pathExpectedImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            return CalcDiffMaskImage(actual, expected, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="pathMaskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(string pathActualImage, string pathExpectedImage, string pathMaskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            using var mask = SKBitmap.Decode(pathMaskImage);
            return CalcDiffMaskImage(actual, expected, mask, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(Stream actualImage, Stream expectedImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            ArgumentNullException.ThrowIfNull(actualImage);

            ArgumentNullException.ThrowIfNull(expectedImage);

            if (actualImage.CanSeek)
            {
                actualImage.Position = 0;
            }
            if (expectedImage.CanSeek)
            {
                expectedImage.Position = 0;
            }

            using var actualImageCopy = new MemoryStream();
            using var expectedImageCopy = new MemoryStream();
            actualImage.CopyTo(actualImageCopy);
            expectedImage.CopyTo(expectedImageCopy);
            actualImageCopy.Position = 0;
            expectedImageCopy.Position = 0;
            using var actual = SKBitmap.Decode(actualImageCopy);
            using var expected = SKBitmap.Decode(expectedImageCopy);
            return CalcDiffMaskImage(actual, expected, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <param name="maskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(Stream actualImage, Stream expectedImage, Stream maskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            ArgumentNullException.ThrowIfNull(actualImage);

            ArgumentNullException.ThrowIfNull(expectedImage);

            ArgumentNullException.ThrowIfNull(maskImage);

            if (actualImage.CanSeek)
            {
                actualImage.Position = 0;
            }
            if (expectedImage.CanSeek)
            {
                expectedImage.Position = 0;
            }

            if (maskImage.CanSeek)
            {
                maskImage.Position = 0;
            }

            using var actualImageCopy = new MemoryStream();
            using var expectedImageCopy = new MemoryStream();
            using var maskCopy = new MemoryStream();
            actualImage.CopyTo(actualImageCopy);
            expectedImage.CopyTo(expectedImageCopy);
            maskImage.CopyTo(maskCopy);
            actualImageCopy.Position = 0;
            expectedImageCopy.Position = 0;
            maskCopy.Position = 0;
            using var actual = SKBitmap.Decode(actualImageCopy);
            using var expected = SKBitmap.Decode(expectedImageCopy);
            using var mask = SKBitmap.Decode(maskCopy);
            return CalcDiffMaskImage(actual, expected, mask, resizeOption, pixelColorShiftTolerance);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(SKBitmap actual, SKBitmap expected, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            var imagesHAveSameDimension = ImagesHaveSameDimension(actual, expected);

            if (resizeOption == ResizeOption.DontResize && !imagesHAveSameDimension)
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            if (imagesHAveSameDimension)
            {
                var maskImage = new SKBitmap(actual.Width, actual.Height);

                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        var actualPixel = actual.GetPixel(x, y);
                        var expectedPixel = expected.GetPixel(x, y);

                        var red = (byte)Math.Abs(actualPixel.Red - expectedPixel.Red);
                        var green = (byte)Math.Abs(actualPixel.Green - expectedPixel.Green);
                        var blue = (byte)Math.Abs(actualPixel.Blue - expectedPixel.Blue);
                        var pixel = new SKColor(red, green, blue);

                        maskImage.SetPixel(x, y, pixel);
                    }
                }
                return maskImage;
            }

            var grown = GrowToSameDimension(actual, expected);
            try
            {
                return CalcDiffMaskImage(grown.Item1, grown.Item2, ResizeOption.DontResize);
            }
            finally
            {
                grown.Item1?.Dispose();
                grown.Item2?.Dispose();
            }
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="mask"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(SKBitmap actual, SKBitmap expected, SKBitmap mask, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0)
        {
            ArgumentNullException.ThrowIfNull(mask);

            var imagesHaveSameDimension = ImagesHaveSameDimension(actual, expected) && ImagesHaveSameDimension(actual, mask);

            if (resizeOption == ResizeOption.DontResize && !imagesHaveSameDimension)
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            if (imagesHaveSameDimension)
            {
                var maskImage = new SKBitmap(actual.Width, actual.Height);

                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        var actualPixel = actual.GetPixel(x, y);
                        var expectedPixel = expected.GetPixel(x, y);
                        var maskPixel = mask.GetPixel(x, y);

                        var red = (byte)(Math.Abs(actualPixel.Red - expectedPixel.Red) - maskPixel.Red);
                        var green = (byte)(Math.Abs(actualPixel.Green - expectedPixel.Green) - maskPixel.Green);
                        var blue = (byte)(Math.Abs(actualPixel.Blue - expectedPixel.Blue) - maskPixel.Blue);
                        var pixel = new SKColor(red, green, blue);

                        maskImage.SetPixel(x, y, pixel);
                    }
                }
                return maskImage;
            }

            var grown = GrowToSameDimension(actual, expected, mask);
            try
            {
                return CalcDiffMaskImage(grown.Item1, grown.Item2, grown.Item3, ResizeOption.DontResize);
            }
            finally
            {
                grown.Item1?.Dispose();
                grown.Item2?.Dispose();
                grown.Item3?.Dispose();
            }
        }

        private static (SKBitmap, SKBitmap) GrowToSameDimension(SKBitmap actual, SKBitmap expected)
        {
            var biggestWidth = actual.Width > expected.Width ? actual.Width : expected.Width;
            var biggestHeight = actual.Height > expected.Height ? actual.Height : expected.Height;
            var skSize = new SKSizeI(biggestWidth, biggestHeight);
            var grownExpected = expected.Resize(skSize, SKFilterQuality.None);
            var grownActual = actual.Resize(skSize, SKFilterQuality.None);

            return (grownActual, grownExpected);
        }

        private static (SKBitmap, SKBitmap, SKBitmap) GrowToSameDimension(SKBitmap actual, SKBitmap expected, SKBitmap mask)
        {
            var biggestWidth = actual.Width > expected.Width ? actual.Width : expected.Width;
            biggestWidth = biggestWidth > mask.Width ? biggestWidth : mask.Width;
            var biggestHeight = actual.Height > expected.Height ? actual.Height : expected.Height;
            biggestHeight = biggestHeight > mask.Height ? biggestHeight : mask.Height;
            var skSize = new SKSizeI(biggestWidth, biggestHeight);
            var grownExpected = expected.Resize(skSize, SKFilterQuality.None);
            var grownActual = actual.Resize(skSize, SKFilterQuality.None);
            var grownMask = mask.Resize(skSize, SKFilterQuality.None);

            return (grownActual, grownExpected, grownMask);
        }
    }
}