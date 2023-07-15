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
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(string pathImageActual, string pathImageExpected, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            using var actualImage = SKBitmap.Decode(pathImageActual);
            using var expectedImage = SKBitmap.Decode(pathImageExpected);
            return ImagesAreEqual(actualImage, expectedImage, resizeOption);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(Stream actual, Stream expected, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            using var actualImage = SKBitmap.Decode(actual);
            using var expectedImage = SKBitmap.Decode(expected);
            return ImagesAreEqual(actualImage, expectedImage, resizeOption);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(SKBitmap actual, SKBitmap expected, ResizeOption resizeOption = ResizeOption.DontResize)
        {
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
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            return CalcDiff(actual, expected, resizeOption);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <param name="resizeOption"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Stream actualImage, Stream expectedImage, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            using var actual = SKBitmap.Decode(actualImage);
            using var expected = SKBitmap.Decode(expectedImage);
            return CalcDiff(actual, expected, resizeOption);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(SKBitmap actual, SKBitmap expected, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            var immagesHaveSameDimension = ImagesHaveSameDimension(actual, expected);

            if (resizeOption == ResizeOption.Resize && !immagesHaveSameDimension)
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
                    absoluteError = absoluteError + r + g + b;

                    pixelErrorCount += r + g + b > 0 ? 1 : 0;
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
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, string pathMaskImage, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            using var mask = SKBitmap.Decode(pathMaskImage);
            return CalcDiff(actual, expected, mask, resizeOption);
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
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(SKBitmap actual, SKBitmap expected, SKBitmap maskImage, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            var immagesHaveSameDimension = ImagesHaveSameDimension(actual, expected) && ImagesHaveSameDimension(actual, maskImage);

            if (resizeOption == ResizeOption.Resize && !immagesHaveSameDimension)
            {
                var grown = GrowToSameDimension(actual, expected, maskImage);
                try
                {
                    return CalcDiff(grown.Item1, grown.Item2, grown.Item3, ResizeOption.DontResize);
                }
                finally
                {
                    grown.Item1?.Dispose();
                    grown.Item2?.Dispose();
                    grown.Item3?.Dispose();
                }
            }

            if (!immagesHaveSameDimension)
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            if (maskImage == null)
            {
                throw new ArgumentNullException(nameof(maskImage));
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

                    absoluteError += error;
                    pixelErrorCount += error > 0 ? 1 : 0;
                }
            }
            var meanError = (double)absoluteError / quantity;
            var pixelErrorPercentage = (double)pixelErrorCount / quantity * 100;
            return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage);
        }

        private static bool ImagesHaveSameDimension(SKBitmap actual, SKBitmap expected)
        {
            if (actual == null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            if (expected == null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

            return actual.Height == expected.Height && actual.Width == expected.Width;
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="resizeOption"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(string pathActualImage, string pathExpectedImage, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            return CalcDiffMaskImage(actual, expected, resizeOption);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <param name="resizeOption"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(Stream actualImage, Stream expectedImage, ResizeOption resizeOption = ResizeOption.DontResize)
        {
            if (actualImage == null)
            {
                throw new ArgumentNullException(nameof(actualImage));
            }

            if (expectedImage == null)
            {
                throw new ArgumentNullException(nameof(expectedImage));
            }

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
            return CalcDiffMaskImage(actual, expected, resizeOption);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="resizeOption"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(SKBitmap actual, SKBitmap expected, ResizeOption resizeOption = ResizeOption.DontResize)
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

        private static (SKBitmap, SKBitmap) GrowToSameDimension(SKBitmap actual, SKBitmap expected)
        {
            var biggesWidh = actual.Width > expected.Width ? actual.Width : expected.Width;
            var biggesHeight = actual.Height > expected.Height ? actual.Height : expected.Height;
            var skSizel = new SKSizeI(biggesWidh, biggesHeight);
            var grownExpected = expected.Resize(skSizel, SKFilterQuality.None);
            var grownActual = actual.Resize(skSizel, SKFilterQuality.None);

            return (grownActual, grownExpected);
        }

        private static (SKBitmap, SKBitmap, SKBitmap) GrowToSameDimension(SKBitmap actual, SKBitmap expected, SKBitmap mask)
        {
            var biggesWidh = actual.Width > expected.Width ? actual.Width : expected.Width;
            biggesWidh = biggesWidh > mask.Width ? biggesWidh : mask.Width;
            var biggesHeight = actual.Height > expected.Height ? actual.Height : expected.Height;
            biggesHeight = biggesHeight > mask.Height ? biggesHeight : mask.Height;
            var skSizel = new SKSizeI(biggesWidh, biggesHeight);
            var grownExpected = expected.Resize(skSizel, SKFilterQuality.None);
            var grownActual = actual.Resize(skSizel, SKFilterQuality.None);
            var grownMask = mask.Resize(skSizel, SKFilterQuality.None);

            return (grownActual, grownExpected, grownMask);
        }
    }
}