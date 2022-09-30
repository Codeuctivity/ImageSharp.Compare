
Compares images
Supports comparing images by using a tolerance mask image.
Linux, Windows, MacOs supported. 

Basic example:

```csharp
bool isEqual = SkiaSharpCompare.ImagesAreEqual("actual.png", "expected.png");

// calcs MeanError, AbsoluteError, PixelErrorCount and PixelErrorPercentage
ICompareResult calcDiff = SkiaSharpCompare.CalcDiff("actual.png", "expected.png");
```
