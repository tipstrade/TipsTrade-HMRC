using System;
using System.Web;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>
  /// Represents a single multi-factor authentication factor used in the Gov-Multi-Factor header.
  /// </summary>
  public class MultiFactor : IFraudPreventionValue {
    /// <summary>Gets or sets the <see cref="MFAMethod"/> being used.</summary>
    public MFAMethod Method { get; set; }

    /// <summary>Gets or sets the timestamp recording the time of the last successful prompt for this factor.</summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets a unique reference identifying a single factor.
    /// For example, a salted-and-hashed phone number used for SMS or an identifier linked
    /// to a TOTP secret – but not the secret itself.
    /// The intention is to recognise the same factor being used across API calls.
    /// </summary>
    public string UniqueReference { get; set; } = "";

    /// <summary>Returns a string that contains the fraud-prevention header value.</summary>
    public string GetHeaderValue() {
      if (string.IsNullOrEmpty(UniqueReference)) throw new InvalidOperationException($"{nameof(UniqueReference)} cannot be empty.");

      var timeEncoded = HttpUtility.UrlEncode($"{TimeStamp.ToUniversalTime():s}Z"); // 2017-04-21T13:23:42Z
      var refEncoded = HttpUtility.UrlEncode(UniqueReference);

      return $"type={Method}&timestamp={timeEncoded}&unique-reference={refEncoded}";
    }
  }
}
