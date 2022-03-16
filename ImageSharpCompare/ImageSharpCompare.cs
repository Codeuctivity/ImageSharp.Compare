using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

namespace Codeuctivity.ImageSharpCompare
{
    /// <summary>
    /// ImageSharpCompare, compares images. Use this class to compare images using a third image as mask of regions where your two compared images may differ. An alpha channel is ignored.
    /// </summary>
    public static class ImageSharpCompare
    {
        private const string sizeDiffersExceptionMessage = "Dimension of images differ";

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="pathImageActual"></param>
        /// <param name="pathImageExpected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(string pathImageActual, string pathImageExpected)
        {
            using var actualImage = Image.Load(pathImageActual);
            using var expectedImage = Image.Load(pathImageExpected);
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
            using var actualImage = Image.Load(actual);
            using var expectedImage = Image.Load(expected);
            return ImagesAreEqual(actualImage, expectedImage);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(Image actual, Image expected)
        {
            if (!ImagesHaveSameDimension(actual, expected))
            {
                return false;
            }

            Image<Rgb24>? actualPixelaccessableImage = ToRgb24Image(actual);
            Image<Rgb24>? expectedPixelaccessableImage = ToRgb24Image(expected);

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    if (!actualPixelaccessableImage[x, y].Equals(expectedPixelaccessableImage[x, y]))
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
            using var actual = Image.Load(pathActualImage);
            using var expected = Image.Load(pathExpectedImage);
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
            using var actual = Image.Load(actualImage);
            using var expected = Image.Load(expectedImage);
            return CalcDiff(actual, expected);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Image actual, Image expected)
        {
            if (!ImagesHaveSameDimension(actual, expected))
            {
                throw new ImageSharpCompareException(sizeDiffersExceptionMessage);
            }

            var actualRgb24 = ToRgb24Image(actual);
            var expectedRgb24 = ToRgb24Image(expected);

            var quantity = actual.Width * actual.Height;
            var absoluteError = 0;
            var pixelErrorCount = 0;

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    var r = Math.Abs(expectedRgb24[x, y].R - actualRgb24[x, y].R);
                    var g = Math.Abs(expectedRgb24[x, y].G - actualRgb24[x, y].G);
                    var b = Math.Abs(expectedRgb24[x, y].B - actualRgb24[x, y].B);
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
            using var actual = Image.Load(pathActualImage);
            using var expected = Image.Load(pathExpectedImage);
            using var mask = Image.Load(pathMaskImage);
            return CalcDiff(actual, expected, mask);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <param name="maskImage"></param>
        /// <returns></returns>
        public static ICompareResult CalcDiff(Stream actualImage, Stream expectedImage, Image maskImage)
        {
            using var actual = Image.Load(actualImage);
            using var expected = Image.Load(expectedImage);
            return CalcDiff(actual, expected, maskImage);
        }

        private static bool ImagesHaveSameDimension(Image actual, Image expected)
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

        private static Image<Rgb24> ToRgb24Image(Image actual)
        {
            if ((actual is Image<Rgb24> actualPixelaccessableImage))
            {
                return actualPixelaccessableImage;
            }

            if (actual is Image<Rgba32> imageRgba32)
            {
                return Rgba32ToRgb24(imageRgba32);
            }

            throw new NotImplementedException($"Pixel type {actual.PixelType} is not supported to be compared.");
        }

        private static Image<Rgb24> Rgba32ToRgb24(Image<Rgba32> imageRgba32)
        {
            var maskRgb24 = new Image<Rgb24>(imageRgba32.Width, imageRgba32.Height);

            for (var x = 0; x < imageRgba32.Width; x++)
            {
                for (var y = 0; y < imageRgba32.Height; y++)
                {
                    var pixel = new Rgb24();
                    pixel.FromRgba32(imageRgba32[x, y]);

                    maskRgb24[x, y] = pixel;
                }
            }
            return maskRgb24;
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images using a image mask for tolerated difference between the two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="maskImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Image actual, Image expected, Image maskImage)
        {
            if (!ImagesHaveSameDimension(actual, expected))
                throw new ImageSharpCompareException(sizeDiffersExceptionMessage);

            if (maskImage == null)
            {
                throw new ArgumentNullException(nameof(maskImage));
            }

            var quantity = actual.Width * actual.Height;
            var absoluteError = 0;
            var pixelErrorCount = 0;

            var actualRgb24 = ToRgb24Image(actual);
            var expectedRgb24 = ToRgb24Image(expected);
            var maskImageRgb24 = ToRgb24Image(maskImage);

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    var maskImagePixel = maskImageRgb24[x, y];
                    var r = Math.Abs(expectedRgb24[x, y].R - actualRgb24[x, y].R);
                    var g = Math.Abs(expectedRgb24[x, y].G - actualRgb24[x, y].G);
                    var b = Math.Abs(expectedRgb24[x, y].B - actualRgb24[x, y].B);

                    var error = 0;

                    if (r > maskImagePixel.R)
                    {
                        error += r;
                    }

                    if (g > maskImagePixel.G)
                    {
                        error += g;
                    }

                    if (b > maskImagePixel.B)
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
        public static Image CalcDiffMaskImage(string pathActualImage, string pathExpectedImage)
        {
            using var actual = Image.Load(pathActualImage);
            using var expected = Image.Load(pathExpectedImage);
            return CalcDiffMaskImage(actual, expected);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actualImage"></param>
        /// <param name="expectedImage"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static Image CalcDiffMaskImage(Stream actualImage, Stream expectedImage)
        {
            using var actual = Image.Load(actualImage);
            using var expected = Image.Load(expectedImage);
            return CalcDiffMaskImage(actual, expected);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static Image CalcDiffMaskImage(Image actual, Image expected)
        {
            if (!ImagesHaveSameDimension(actual, expected))
            {
                throw new ImageSharpCompareException(sizeDiffersExceptionMessage);
            }

            var actualRgb24 = ToRgb24Image(actual);
            var expectedRgb24 = ToRgb24Image(expected);

            var maskImage = new Image<Rgb24>(actual.Width, actual.Height);

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    var pixel = new Rgb24();

                    pixel.R = (byte)Math.Abs(actualRgb24[x, y].R - expectedRgb24[x, y].R);
                    pixel.G = (byte)Math.Abs(actualRgb24[x, y].G - expectedRgb24[x, y].G);
                    pixel.B = (byte)Math.Abs(actualRgb24[x, y].B - expectedRgb24[x, y].B);

                    maskImage[x, y] = pixel;
                }
            }
            return maskImage;
        }
    }
}