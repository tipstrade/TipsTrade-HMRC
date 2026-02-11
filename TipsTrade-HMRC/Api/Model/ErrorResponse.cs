using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a response containing error information.</summary>
  public class ErrorResponse {
    /// <summary>A machine-readable error code. This is unique for each error scenario.</summary>
    [JsonProperty("code"), JsonPropertyName("code")]
    public string Code { get; set; }

    /// <summary>A list of errors that occurred.</summary>
    [JsonProperty("errors"), JsonPropertyName("errors")]
    public ErrorResponse[] Errors { get; set; }

    /// <summary>A human-readable explanation for the error.</summary>
    [JsonProperty("message"), JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>The path that caused the error to occur.</summary>
    [JsonProperty("path"), JsonPropertyName("path")]
    public string Path { get; set; }

    /// <summary>The paths that caused the error to occur.</summary>
    [JsonProperty("paths"), JsonPropertyName("paths")]
    public IEnumerable<string> Paths { get; set; }  

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"{Message}";
    }

    /// <summary>Creates a <see cref="ErrorResponse"/> from the specified JSON returned by an OAuth2 function.</summary>
    public static ErrorResponse FromOAuth2Error(string json) {
      var template = new {
        error = "",
        error_description = ""
      };

      template = JsonConvert.DeserializeAnonymousType(json, template);

      if (string.IsNullOrEmpty(template.error) && string.IsNullOrEmpty(template.error_description)) {
        return null;
      } else {
        return new ErrorResponse() {
          Code = template.error,
          Message = template.error_description
        };
      }
    }
  }
}
