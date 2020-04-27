using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Codeuctivity
{
    /// <summary>
    /// ImageSharpCompare, inspired by testapi feature to compare images. Use this class to compare images using a third image as mask of regions where your two compared images may differ.
    /// </summary>
    public static class ImageSharpCompare
    {
        private const string sizeDiffersExceptionMessage = "Dimension of images differ";

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImageAreEqual(string actual, string expected)
        {
            using (var actualImage = (Image<Rgba32>)Image.Load(actual))
            using (var expectedImage = (Image<Rgba32>)Image.Load(expected))
            {
                return ImageAreEqual(actualImage, expectedImage);
            }
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImageAreEqual(Image<Rgba32> actual, Image<Rgba32> expected)
        {
            if (!ImagesHaveSameDimension(actual, expected))
            {
                return false;
            }

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    if (actual[x, y] != expected[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool ImagesHaveSameDimension(Image<Rgba32> actual, Image<Rgba32> expected)
        {
            if (actual == null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            if (expected == null)
            {
                throw new ArgumentNullException(nameof(expected));
            }
            return (actual.Height == expected.Height && actual.Width == expected.Width);
        }

        /// <summary>
        /// Compares two images for equivalence using a mask image for tolerated difference between the two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="pathMaskImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, string pathMaskImage)
        {
            using (var actual = (Image<Rgba32>)Image.Load(pathActualImage))
            using (var expected = (Image<Rgba32>)Image.Load(pathExpectedImage))
            using (var mask = (Image<Rgba32>)Image.Load(pathMaskImage))
            {
                return CalcDiff(actual, expected, mask);
            }
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage)
        {
            using (var actual = (Image<Rgba32>)Image.Load(pathActualImage))
            using (var expected = (Image<Rgba32>)Image.Load(pathExpectedImage))
            {
                return CalcDiff(actual, expected);
            }
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Image<Rgba32> actual, Image<Rgba32> expected)
        {
            if (ImagesHaveSameDimension(actual, expected))
            {
                var quantity = actual.Width * actual.Height;
                var absoluteError = 0;
                var pixelErrorCount = 0;
                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        var r = Math.Abs(expected[x, y].R - actual[x, y].R);
                        var g = Math.Abs(expected[x, y].G - actual[x, y].G);
                        var b = Math.Abs(expected[x, y].B - actual[x, y].B);

                        absoluteError = absoluteError + r + g + b;

                        pixelErrorCount += (r + g + b) > 0 ? 1 : 0;
                    }
                }
                var meanError = absoluteError / quantity;
                var pixelErrorPercentage = ((double)pixelErrorCount / quantity) * 100;
                return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage);
            }
            throw new ImageSharpCompareException(sizeDiffersExceptionMessage);
        }

        /// <summary>
        /// Compares two images for equivalence using a mask image for tolerated difference between the two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="maskImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Image<Rgba32> actual, Image<Rgba32> expected, Image<Rgba32> maskImage)
        {
            if (ImagesHaveSameDimension(actual, expected))
            {
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
                        var maskImagePixel = maskImage[x, y];
                        var r = Math.Abs(expected[x, y].R - actual[x, y].R);
                        var g = Math.Abs(expected[x, y].G - actual[x, y].G);
                        var b = Math.Abs(expected[x, y].B - actual[x, y].B);

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
                var meanError = absoluteError / quantity;
                var pixelErrorPercentage = ((double)pixelErrorCount / quantity) * 100;
                return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage);
            }
            throw (new ImageSharpCompareException(sizeDiffersExceptionMessage));
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static Image<Rgba32> CalcDiffMaskImage(string pathActualImage, string pathExpectedImage)
        {
            using (var actual = (Image<Rgba32>)Image.Load(pathActualImage))
            using (var expected = (Image<Rgba32>)Image.Load(pathExpectedImage))
            {
                return CalcDiffMaskImage(actual, expected);
            }
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static Image<Rgba32> CalcDiffMaskImage(Image<Rgba32> actual, Image<Rgba32> expected)
        {
            if (ImagesHaveSameDimension(actual, expected))
            {
                var maskImage = new Image<Rgba32>(actual.Width, actual.Height);

                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        var r = Math.Abs(expected[x, y].R - actual[x, y].R);
                        var g = Math.Abs(expected[x, y].G - actual[x, y].G);
                        var b = Math.Abs(expected[x, y].B - actual[x, y].B);
                        maskImage[x, y] = new Rgba32(r, g, b);
                    }
                }
                return maskImage;
            }
            throw (new ImageSharpCompareException(sizeDiffersExceptionMessage));
        }
    }
}