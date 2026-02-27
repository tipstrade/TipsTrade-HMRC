using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  /// <summary>Represents a result containing a Fraud Prevention validation.</summary>
  public class ValidateResult {
    /// <summary>The validation code.</summary>
    [JsonProperty("code"), JsonPropertyName("code")]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter)), System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
    public ValidationCode Code { get; set; }

    /// <summary>Array of errors.</summary>
    [JsonProperty("errors"), JsonPropertyName("errors")]
    public IEnumerable<ValidateError> Errors { get; set; } = new ValidateError[] { };

    /// <summary>The validation message.</summary>
    [JsonProperty("message"), JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>The version of fraud prevention headers that request was validated against.</summary>
    [JsonProperty("specVersion"), JsonPropertyName("specVersion")]
    public string SpecVersion { get; set; }

    /// <summary>Array of warnings.</summary>
    [JsonProperty("warnings"), JsonPropertyName("warnings")]
    public IEnumerable<ValidateError> Warnings { get; set; } = new ValidateError[] { };

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"{Errors?.Count() ?? 0} errors, {Warnings?.Count() ?? 0} warnings";
    }
  }
}
