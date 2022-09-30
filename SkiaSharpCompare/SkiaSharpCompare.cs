using SkiaSharp;
using System;
using System.IO;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// SkiaSharpCompare, compares images. Use this class to compare images using a third image as mask of regions where your two compared images may differ. An alpha channel is ignored.
    /// </summary>
    public static class SkiaSharpCompare
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
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(string pathImageActual, string pathImageExpected)
        {
            using var actualImage = SKBitmap.Decode(pathImageActual);
            using var expectedImage = SKBitmap.Decode(pathImageExpected);
            return ImagesAreEqual(actualImage, expectedImage);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(Stream actual, Stream expected)
        {
            using var actualImage = SKBitmap.Decode(actual);
            using var expectedImage = SKBitmap.Decode(expected);
            return ImagesAreEqual(actualImage, expectedImage);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(SKBitmap actual, SKBitmap expected)
        {
            if (!ImagesHaveSameDimension(actual, expected))
            {
                return false;
            }

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

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            return CalcDiff(actual, expected);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Stream actualImage, Stream expectedImage)
        {
            using var actual = SKBitmap.Decode(actualImage);
            using var expected = SKBitmap.Decode(expectedImage);
            return CalcDiff(actual, expected);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(SKBitmap actual, SKBitmap expected)
        {
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
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, string pathMaskImage)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            using var mask = SKBitmap.Decode(pathMaskImage);
            return CalcDiff(actual, expected, mask);
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
        /// Calculates ICompareResult expressing the amount of difference of both images using a image mask for tolerated difference between the two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="maskImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(SKBitmap actual, SKBitmap expected, SKBitmap maskImage)
        {
            if (!ImagesHaveSameDimension(actual, expected))
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

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(string pathActualImage, string pathExpectedImage)
        {
            using var actual = SKBitmap.Decode(pathActualImage);
            using var expected = SKBitmap.Decode(pathExpectedImage);
            return CalcDiffMaskImage(actual, expected);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(Stream actualImage, Stream expectedImage)
        {
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
            return CalcDiffMaskImage(actual, expected);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(SKBitmap actual, SKBitmap expected)
        {
            if (!ImagesHaveSameDimension(actual, expected))
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

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
    }
}