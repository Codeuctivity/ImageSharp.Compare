# SkiaSharpCompare

Compares images

[![.NET build and test](https://github.com/Codeuctivity/SkiaSharp.Compare/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/SkiaSharp.Compare/actions/workflows/dotnet.yml) [![Nuget](https://img.shields.io/nuget/v/Codeuctivity.SkiaSharpCompare.svg)](https://www.nuget.org/packages/Codeuctivity.SkiaSharpCompare/) [![Donate](https://img.shields.io/static/v1?label=Paypal&message=Donate&color=informational)](https://www.paypal.com/donate?hosted_button_id=7M7UFMMRTS7UE)

Inspired by the image compare feature "Visual verification API" of [TestApi](https://blogs.msdn.microsoft.com/ivo_manolov/2009/04/20/introduction-to-testapi-part-3-visual-verification-apis/) this code supports comparing images by using a tolerance mask image. That tolerance mask image is a valid image by itself and can be manipulated.

SkiaSharpCompare focus on os agnostic support and therefore depends on [SkiaSharp](https://github.com/mono/SkiaSharp).

NOTE: The Alpha-channel is ignored.

## Example simple show cases

### Compares each RGB value of each pixel to determine the equality

```csharp
bool isEqual = Compare.ImagesAreEqual("actual.png", "expected.png");
```

### Calculates diff

```csharp
var calcDiff = Compare.CalcDiff("2x2PixelBlack.png", "2x2PixelWhite.png");
Console.WriteLine($"PixelErrorCount: {diff.PixelErrorCount}");
Console.WriteLine($"PixelErrorPercentage: {diff.PixelErrorPercentage}");
Console.WriteLine($"AbsoluteError: {diff.AbsoluteError}");
Console.WriteLine($"MeanError: {diff.MeanError}");
// PixelErrorCount: 4
// PixelErrorPercentage: 100
// AbsoluteError: 3060
// MeanError: 765
```

## Example show case allowing some tolerated diff

Imagine two images you want to compare, and want to accept the found difference as at state of allowed difference.

### Reference Image

![actual image](./SkiaSharpCompareTestNunit/TestData/Calc0.jpg "Reference Image")

### Actual Image

![actual image](./SkiaSharpCompareTestNunit/TestData/Calc1.jpg "Reference Image")

### Tolerance mask image

Using **CalcDiffMaskImage** you can calc a diff mask from actual and reference image

Example - Create difference image

```csharp
using (var fileStreamDifferenceMask = File.Create("differenceMask.png"))
using (var maskImage = SkiaSharpCompare.CalcDiffMaskImage(pathPic1, pathPic2))
var encodedData = maskImage.Encode(SKEncodedImageFormat.Png, 100);
encodedData.SaveTo(fileStreamDifferenceMask);
```

![differenceMask.png](./SkiaSharpCompareTestNunit/TestData/differenceMask.png "differenceMask.png")

Example - Compare two images using the created difference image. Add white pixels to differenceMask.png where you want to allow difference.

```csharp
var maskedDiff = SkiaSharpCompare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(0));
```
