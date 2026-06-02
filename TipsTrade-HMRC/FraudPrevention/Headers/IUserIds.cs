using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-User-IDs header.</summary>
  public interface IUserIds {
    /// <summary>Gets or sets the accounts the user holds.</summary>
    Dictionary<string, string>? UserIds { get; set; }
  }

  internal static class UserIdsExtensions {
    internal static (string Name, string Value) GetUserIds(this IUserIds source) {
      // As per documentation, both the key and value of the dictionary should be URL encoded.
      var value = source.UserIds == null ? "" : source.UserIds.EncodeKeyValues(true);

      return ("Gov-Client-User-IDs", value);
    }
  }
}
