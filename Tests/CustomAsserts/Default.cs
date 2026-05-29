using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  internal static class AssertExtensions {
    /// <summary>Verifies that the object is equal to the Type's default value.</summary>
    public static void Default<T>(T actual) {
      Assert.That(actual, Is.EqualTo(default(T)));
    }

    /// <summary>Verifies that the object is not equal to the Type's default value.</summary>
    public static void NotDefault<T>(T actual) {
      Assert.That(actual, Is.Not.EqualTo(default(T)));
    }
  }
}
