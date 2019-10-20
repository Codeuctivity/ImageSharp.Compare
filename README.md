# ImageSharpCompare

Asserts Images

Inspired by the image compare feature "Visual verification API" of [TestApi](https://blogs.msdn.microsoft.com/ivo_manolov/2009/04/20/introduction-to-testapi-part-3-visual-verification-apis/) this code supports comparing images by using a tolerance mask image. That tolerance mask image is a valid image by itself and can be manipulated.

ImageSharpCompare focus on os indepent support and therfore depends on  [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp). https://devblogs.microsoft.com/dotnet/net-core-image-processing/ gave me som 

```PowerShell
Install-Package ImageSharpCompare
```

Example - Create difference image

```csharp
compare = new Codeuctivity.ImageSharpCompare();
using (var fileStreamDifferenceMask = File.Create("differenceMask.png"))
using (var maskImage = compare.CalcDiffMaskImage(pathPic1, pathPic2))
    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
```

Example - Compare two images using the created difference image

```csharp
var maskedDiff = compare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(0));
```
