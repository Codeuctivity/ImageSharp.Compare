using NUnit.Framework;
using SixLabors.ImageSharp;
using System;
using System.Reflection;

namespace ImageSharpCompareTestNunit
{
    internal static class AssertDisposeBehavior
    {

        internal static void AssertThatImageIsDisposed(Image image, bool expectedDisposeState = false)
        {
            const string imageSharpPrivateFieldNameIsDisposed = "isDisposed";
            var isDisposed = (bool?)GetInstanceField(image, imageSharpPrivateFieldNameIsDisposed);
            Assert.That(isDisposed, Is.EqualTo(expectedDisposeState));
            image.Dispose();
            isDisposed = (bool?)GetInstanceField(image, imageSharpPrivateFieldNameIsDisposed);
            Assert.That(isDisposed, Is.True);
        }

        private static object? GetInstanceField<T>(T instance, string fieldName)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = typeof(T).GetField(fieldName, bindFlags);
            return field == null ? throw new ArgumentNullException(fieldName) : field.GetValue(instance);
        }
    }
}