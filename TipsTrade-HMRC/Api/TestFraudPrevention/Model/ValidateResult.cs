using System.Collections.Generic;
using System.Linq;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  /// <summary>Represents a result containing a Fraud Prevention validation.</summary>
  public class ValidateResult {
    /// <summary>The validation code.</summary>
    public ValidationCode Code { get; set; }

    /// <summary>Array of errors.</summary>
    public IEnumerable<ValidateError> Errors { get; set; } = new ValidateError[] { };

    /// <summary>The validation message.</summary>
    public string Message { get; set; }

    /// <summary>The version of fraud prevention headers that request was validated against.</summary>
    public string SpecVersion { get; set; }

    /// <summary>Array of warnings.</summary>
    public IEnumerable<ValidateError> Warnings { get; set; } = new ValidateError[] { };

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"{Errors?.Count() ?? 0} errors, {Warnings?.Count() ?? 0} warnings";
    }
  }
}
