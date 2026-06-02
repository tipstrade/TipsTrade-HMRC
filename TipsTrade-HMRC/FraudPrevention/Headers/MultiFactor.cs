using System;

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

    /// <inheritdoc/>
    public string GetHeaderValue() {
      var timeEncoded = Uri.EscapeDataString(TimeStamp.EncodeTimestamp());
      var refEncoded = Uri.EscapeDataString(UniqueReference ?? "");

      return $"type={Method}&timestamp={timeEncoded}&unique-reference={refEncoded}";
    }
  }
}
