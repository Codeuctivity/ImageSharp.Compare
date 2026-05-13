
Compares images
Supports comparing images by using a tolerance mask image.
Linux, Windows, MacOs supported. 

## ImageSharp License Note

If you run into the licensing hassle around the ImageSharp dependency, consider [Codeuctivity/SkiaSharp.Compare](https://github.com/Codeuctivity/SkiaSharp.Compare) as an alternative. It is semi API compatible, and if that project already satisfies your personal needs, it may also be the better fit for you. For further details, see [Licence Enforcement Changes](https://sixlabors.com/posts/licence-enforcement-changes/). The respect here is directed toward the ImageSharp dependency and the work behind it. Keeping this project compliant with license changes takes ongoing effort, and [funding](https://github.com/sponsors/Codeuctivity) would help make it easier to upgrade to the latest ImageSharp versions and keep pace with their technical changes responsibly.

Basic example:

```csharp
bool isEqual = ImageSharpCompare.ImagesAreEqual("actual.png", "expected.png");

// calcs MeanError, AbsoluteError, PixelErrorCount and PixelErrorPercentage
ICompareResult calcDiff = ImageSharpCompare.CalcDiff("actual.png", "expected.png");
```
