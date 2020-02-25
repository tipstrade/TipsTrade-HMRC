using System.Collections.Generic;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  /// <summary>Represents an error returned during Fraud Prevention validation.</summary>
  public class ValidateError {
    /// <summary>The error code.</summary>
    public ValidationCode Code { get; set; }

    /// <summary>Array of headers the error applies to.</summary>
    public IEnumerable<string> Headers { get; set; }

    /// <summary>The error message.</summary>
    public string Message { get; set; }

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"{Code}: {Message}";
    }
  }
}
