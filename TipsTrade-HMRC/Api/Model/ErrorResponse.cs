using Newtonsoft.Json;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a response containing error information.</summary>
  public class ErrorResponse {
    /// <summary>A machine-readable error code. This is unique for each error scenario.</summary>
    public string Code { get; set; }

    /// <summary>A list of errors that occurred.</summary>
    public ErrorResponse[] Errors { get; set; }

    /// <summary>A human-readable explanation for the error.</summary>
    public string Message { get; set; }

    /// <summary>The path that caused the error to occur.</summary>
    public string Path { get; set; }

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
