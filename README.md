# ImageSharpCompare

[![Build status](https://ci.appveyor.com/api/projects/status/yr17qicavvbfypra?svg=true)](https://ci.appveyor.com/project/stesee/imagesharp-compare) [![Nuget](https://img.shields.io/nuget/v/ImageSharpCompare.svg)](https://www.nuget.org/packages/ImageSharpCompare/) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/e61c3debbfeb48469dadcc6109c719c6)](https://www.codacy.com/manual/stesee/ImageSharp.Compare?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Codeuctivity/ImageSharp.Compare&amp;utm_campaign=Badge_Grade)

Compares images

Inspired by the image compare feature "Visual verification API" of [TestApi](https://blogs.msdn.microsoft.com/ivo_manolov/2009/04/20/introduction-to-testapi-part-3-visual-verification-apis/) this code supports comparing images by using a tolerance mask image. That tolerance mask image is a valid image by itself and can be manipulated.

ImageSharpCompare focus on os indepent support and therfore depends on  [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp).

```PowerShell
Install-Package ImageSharpCompare
```

## Example show case

Imagine two images you want to compare, and want to accept the found difference as at state of allowed difference.

### Reference Image

![actual image](./ImageSharpCompareTestNunit/TestData/Calc0.jpg "Refernce Image")

### Actual Image

![actual image](./ImageSharpCompareTestNunit/TestData/Calc1.jpg "Refernce Image")

### Tolerance mask image

using "compare.CalcDiff" you can calc a diff mask from actual and reference image

Example - Create difference image

```csharp
compare = new Codeuctivity.ImageSharpCompare();
using (var fileStreamDifferenceMask = File.Create("differenceMask.png"))
using (var maskImage = compare.CalcDiffMaskImage(pathPic1, pathPic2))
    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
```

![differenceMask.png](./ImageSharpCompareTestNunit/TestData/differenceMask.png "differenceMask.png")

Example - Compare two images using the created difference image. Add white pixels to  differenceMask.png where you want to allow difference.

```csharp
var maskedDiff = compare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(0));
```
