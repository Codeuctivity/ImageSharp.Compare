using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;

using SixLabors.ImageSharp;

namespace Codeuctivity.ImageSharpCompare;
public static partial class ImageSharpCompare
{
    /// <summary>
    /// Creates an image with a diff mask based on another one
    /// </summary>
    /// <param name="pathActualImage"></param>
    /// <param name="pathExpectedImage"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(string pathActualImage, string pathExpectedImage, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        using var actual = Image.Load(pathActualImage);
        using var expected = Image.Load(pathExpectedImage);
        return CalcDiffImageWithMask(actual, expected, resizeOption, diffColor);
    }

    /// <summary>
    /// Creates an image with a diff mask based on another one
    /// </summary>
    /// <param name="pathActualImage"></param>
    /// <param name="pathExpectedImage"></param>
    /// <param name="pathMaskImage"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(string pathActualImage, string pathExpectedImage, string pathMaskImage, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        using var actual = Image.Load(pathActualImage);
        using var expected = Image.Load(pathExpectedImage);
        using var mask = Image.Load(pathMaskImage);
        return CalcDiffImageWithMask(actual, expected, mask, resizeOption, diffColor);
    }

    /// <summary>
    /// Creates an image with a diff mask based on another one
    /// </summary>
    /// <param name="actualImage"></param>
    /// <param name="expectedImage"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(Stream actualImage, Stream expectedImage, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        using var actual = Image.Load(actualImage);
        using var expected = Image.Load(expectedImage);
        return CalcDiffImageWithMask(actual, expected, resizeOption, diffColor);
    }

    /// <summary>
    /// Creates an image with a diff mask based on another one
    /// </summary>
    /// <param name="actualImage"></param>
    /// <param name="expectedImage"></param>
    /// <param name="maskImage"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(Stream actualImage, Stream expectedImage, Stream maskImage, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        using var actual = Image.Load(actualImage);
        using var expected = Image.Load(expectedImage);
        using var mask = Image.Load(maskImage);
        return CalcDiffImageWithMask(actual, expected, mask, resizeOption, diffColor);
    }

    /// <summary>
    /// Creates an image with a diff mask based on another one
    /// </summary>
    /// <param name="actual"></param>
    /// <param name="expected"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(Image actual, Image expected, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        ArgumentNullException.ThrowIfNull(actual);

        ArgumentNullException.ThrowIfNull(expected);

        var ownsActual = false;
        var ownsExpected = false;
        Image<Rgb24>? actualRgb24 = null;
        Image<Rgb24>? expectedRgb24 = null;

        try
        {
            actualRgb24 = ImageSharpPixelTypeConverter.ToRgb24Image(actual, out ownsActual);
            expectedRgb24 = ImageSharpPixelTypeConverter.ToRgb24Image(expected, out ownsExpected);

            return CalcDiffImageWithMask(actualRgb24, expectedRgb24, resizeOption, diffColor);
        }
        finally
        {
            if (ownsActual)
            {
                actualRgb24?.Dispose();
            }
            if (ownsExpected)
            {
                expectedRgb24?.Dispose();
            }
        }
    }

    /// <summary>
    /// Creates an image with a diff mask based on another one
    /// </summary>
    /// <param name="actual"></param>
    /// <param name="expected"></param>
    /// <param name="mask"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(Image actual, Image expected, Image mask, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        ArgumentNullException.ThrowIfNull(actual);
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(mask);
        var ownsActual = false;
        var ownsExpected = false;
        var ownsMask = false;
        Image<Rgb24>? actualRgb24 = null;
        Image<Rgb24>? expectedRgb24 = null;
        Image<Rgb24>? maskRgb24 = null;

        try
        {
            actualRgb24 = ImageSharpPixelTypeConverter.ToRgb24Image(actual, out ownsActual);
            expectedRgb24 = ImageSharpPixelTypeConverter.ToRgb24Image(expected, out ownsExpected);
            maskRgb24 = ImageSharpPixelTypeConverter.ToRgb24Image(mask, out ownsMask);

            return CalcDiffImageWithMask(actualRgb24, expectedRgb24, maskRgb24, resizeOption, diffColor);
        }
        finally
        {
            if (ownsActual)
            {
                actualRgb24?.Dispose();
            }
            if (ownsExpected)
            {
                expectedRgb24?.Dispose();
            }
            if (ownsMask)
            {
                maskRgb24?.Dispose();
            }
        }
    }

    /// <summary>
    /// Creates an image with a diff mask based on another one
    /// </summary>
    /// <param name="actual"></param>
    /// <param name="expected"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(Image<Rgb24> actual, Image<Rgb24> expected, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        var imagesHAveSameDimension = ImagesHaveSameDimension(actual, expected);

        if (resizeOption == ResizeOption.DontResize && !imagesHAveSameDimension)
        {
            throw new ImageSharpCompareException(sizeDiffersExceptionMessage);
        }

        if (imagesHAveSameDimension)
        {
            var maskImage = new Image<Rgb24>(actual.Width, actual.Height);

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    var actualPixel = actual[x, y];
                    var expectedPixel = expected[x, y];

                    var isEqual =
                        Math.Abs(actualPixel.R - expectedPixel.R) == 0 &&
                        Math.Abs(actualPixel.G - expectedPixel.G) == 0 &&
                        Math.Abs(actualPixel.B - expectedPixel.B) == 0;

                    maskImage[x, y] = isEqual ? actualPixel : (diffColor ?? Color.Fuchsia).ToPixel<Rgb24>();
                }
            }
            return maskImage;
        }

        var grown = GrowToSameDimension(actual, expected);
        try
        {
            return CalcDiffImageWithMask(grown.Item1, grown.Item2, ResizeOption.DontResize, diffColor);
        }
        finally
        {
            grown.Item1?.Dispose();
            grown.Item2?.Dispose();
        }
    }

    /// <summary>
    /// Creates an image with a diff mask based on another one using a image mask for tolerated difference between the two images.
    /// </summary>
    /// <param name="actual"></param>
    /// <param name="expected"></param>
    /// <param name="mask"></param>
    /// <param name="resizeOption"></param>
    /// <param name="diffColor">Default value is Color.Fuchsia</param>
    /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
    public static Image CalcDiffImageWithMask(Image<Rgb24> actual, Image<Rgb24> expected, Image<Rgb24> mask, ResizeOption resizeOption = ResizeOption.DontResize, Color? diffColor = null)
    {
        ArgumentNullException.ThrowIfNull(mask);
        var imagesHaveSameDimensions = ImagesHaveSameDimension(actual, expected) && ImagesHaveSameDimension(actual, mask);

        if (!imagesHaveSameDimensions && resizeOption == ResizeOption.DontResize)
        {
            throw new ImageSharpCompareException(sizeDiffersExceptionMessage);
        }

        if (imagesHaveSameDimensions)
        {
            var maskImageResult = new Image<Rgb24>(actual.Width, actual.Height);

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    var maskPixel = mask[x, y];
                    var actualPixel = actual[x, y];
                    var expectedPixel = expected[x, y];

                    var isEqual =
                        Math.Max(byte.MinValue, Math.Abs(expectedPixel.R - actualPixel.R) - maskPixel.R) == 0 &&
                        Math.Max(byte.MinValue, Math.Abs(expectedPixel.G - actualPixel.G) - maskPixel.G) == 0 &&
                        Math.Max(byte.MinValue, Math.Abs(expectedPixel.B - actualPixel.B) - maskPixel.B) == 0;

                    maskImageResult[x, y] = isEqual ? actualPixel : (diffColor ?? Color.Fuchsia).ToPixel<Rgb24>();
                }
            }

            return maskImageResult;
        }

        var grown = GrowToSameDimension(actual, expected, mask);
        try
        {
            return CalcDiffImageWithMask(grown.Item1, grown.Item2, grown.Item3, ResizeOption.DontResize, diffColor);
        }
        finally
        {
            grown.Item1?.Dispose();
            grown.Item2?.Dispose();
            grown.Item3?.Dispose();
        }
    }
}
