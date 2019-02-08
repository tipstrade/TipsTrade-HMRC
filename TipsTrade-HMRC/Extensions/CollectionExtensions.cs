namespace System.Collections {
  internal static class CollectionExtensions {
    public static bool Any(this IEnumerable value) {
      if (value == null) throw new ArgumentNullException();

      foreach (var item in value) {
        return true;
      }
      return false;
    }
  }
}
