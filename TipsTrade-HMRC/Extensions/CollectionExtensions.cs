using System.Collections.Generic;
using System.Text;
using System.Web;

namespace System.Collections {
  internal static class CollectionExtensions {
    public static bool Any(this IEnumerable value) {
      if (value == null) throw new ArgumentNullException();

      foreach (var item in value) {
        return true;
      }
      return false;
    }

    public static string GetHeaderValue<T>(this IDictionary<string, T> values) {
      var sb = new StringBuilder();

      foreach (var item in values) {
        if (sb.Length != 0) {
          sb.Append("&");
        }

        sb.AppendFormat("{0}={1}", HttpUtility.UrlEncode($"{item.Key}"), HttpUtility.UrlEncode($"{item.Value}"));
      }

      return sb.ToString();
    }
  }
}
