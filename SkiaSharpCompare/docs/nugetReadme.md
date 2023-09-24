# Compares images

## Compares each pixel to determine the equality

```csharp
bool isEqual = Compare.ImagesAreEqual("actual.png", "expected.png");
```

## Calculates diff

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

## Further features

- Diff mask: allowing defined areas to diff from the compared image.
- Compare images that have different dimension
- Linux, MacOs and Windows supported
