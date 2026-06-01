using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-User-IDs header.</summary>
  public interface IUserIds {
    /// <summary>Gets or sets the accounts the user holds.</summary>
    Dictionary<string, string>? UserIds { get; set; }
  }

  internal static class UserIdsExtensions {
    internal static FraudPreventionHeader GetUserIdsHeader(this IUserIds source) =>
      new FraudPreventionHeader("Gov-Client-User-IDs", true, source.UserIds);
  }
}
