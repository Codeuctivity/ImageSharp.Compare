namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// Options that are applied if images do have different image dimension
    /// </summary>
    public enum ResizeOption
    {
        /// <summary>
        /// Dont resize images with different size.
        /// </summary>
        DontResize,

        /// <summary>
        /// Images with different size will get resized before pixel based compare is used to determine equality.
        /// </summary>
        Resize
    }
}