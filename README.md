# ImageSharpCompare

Compares images

[![.github/workflows/dotnet.yml](https://github.com/Codeuctivity/ImageSharp.Compare/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/ImageSharp.Compare/actions/workflows/dotnet.yml) [![Nuget](https://img.shields.io/nuget/v/Codeuctivity.ImageSharpCompare.svg)](https://www.nuget.org/packages/Codeuctivity.ImageSharpCompare/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/2fb2807a16f84021b088769e56e1515a)](https://www.codacy.com/gh/Codeuctivity/ImageSharp.Compare/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/ImageSharp.Compare&utm_campaign=Badge_Grade) [![Donate](https://img.shields.io/static/v1?label=Paypal&message=Donate&color=informational)](https://www.paypal.com/donate?hosted_button_id=7M7UFMMRTS7UE)

Inspired by the image compare feature "Visual verification API" of [TestApi](https://blogs.msdn.microsoft.com/ivo_manolov/2009/04/20/introduction-to-testapi-part-3-visual-verification-apis/) this code supports comparing images by using a tolerance mask image. That tolerance mask image is a valid image by itself and can be manipulated.

ImageSharpCompare focus on os agnostic support and therefore depends on [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp).

## Example simple show cases

```csharp
bool isEqual = ImageSharpCompare.ImagesAreEqual("actual.png", "expected.png");

var calcDiff = ImageSharpCompare.CalcDiff("2x2PixelBlack.png", "2x2PixelWhite.png");
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

![actual image](./ImageSharpCompareTestNunit/TestData/Calc0.jpg "Reference Image")

### Actual Image

![actual image](./ImageSharpCompareTestNunit/TestData/Calc1.jpg "Reference Image")

### Tolerance mask image

using "compare.CalcDiff" you can calc a diff mask from actual and reference image

Example - Create difference image

```csharp
using (var fileStreamDifferenceMask = File.Create("differenceMask.png"))
using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(pathPic1, pathPic2))
    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
```

![differenceMask.png](./ImageSharpCompareTestNunit/TestData/differenceMask.png "differenceMask.png")

Example - Compare two images using the created difference image. Add white pixels to differenceMask.png where you want to allow difference.

```csharp
var maskedDiff = ImageSharpCompare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(0));
```

## .net Framework specific note

.net framework (an older Windows-based .NET implementation) is supported by versions 2.x.y .
