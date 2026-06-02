using System;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-User-Agent header.</summary>
  public interface IUserAgent {
    /// <summary>
    /// Gets or sets the operating system family, version, device manufacturer and model of the originating device.
    /// </summary>
    UserAgent? UserAgent { get; set; }
  }

  /// <summary>
  /// Extension methods for IUserAgent.
  /// </summary>
  public static class UserAgentExtensions {
    internal static (string Name, string Value) GetUserAgent(this IUserAgent source) {
      return ("Gov-Client-User-Agent", source.UserAgent?.GetHeaderValue() ?? "");
    }

    /// <summary>
    /// Populates the UserAgent properties with the operating system family and version of the originating device.
    /// </summary>
    /// <param name="userAgent"></param>
    public static void PopulateUserAgent(this IUserAgent userAgent) {
      var os = Environment.OSVersion;
      userAgent.UserAgent = new UserAgent {
        OSFamily = $"{os.Platform}",
        OSVersion = os.VersionString
      };
    }
  }
}
