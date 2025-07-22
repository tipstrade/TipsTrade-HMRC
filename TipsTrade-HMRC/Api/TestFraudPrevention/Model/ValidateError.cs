using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  /// <summary>Represents an error returned during Fraud Prevention validation.</summary>
  public class ValidateError {
    /// <summary>The error code.</summary>
    [JsonProperty("code"), JsonPropertyName("code")]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter)), System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
    public ValidationCode Code { get; set; }

    /// <summary>Array of headers the error applies to.</summary>
    [JsonProperty("headers"), JsonPropertyName("headers")]
    public IEnumerable<string> Headers { get; set; }

    /// <summary>The error message.</summary>
    [JsonProperty("message"), JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"{Code}: {Message}";
    }
  }
}
