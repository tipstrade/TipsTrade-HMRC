namespace Xunit {
  partial class Assert {
    /// <summary>Verifies that the object is equal to the Type's default value, using a default comparer.</summary>
    /// <typeparam name="T">The type of the object to be compared</typeparam>
    /// <param name="actual">The actual object</param>
    public static void Default<T>(T actual) {
      Equal(default(T), actual);
    }

    /// <summary>Verifies that the object is not equal to the Type's default value, using a default comparer.</summary>
    /// <typeparam name="T">The type of the object to be compared</typeparam>
    /// <param name="actual">The actual object</param>
    public static void NotDefault<T>(T actual) {
      NotEqual(default(T), actual);
    }
  }
}
