namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a response containing error information.</summary>
  public class ErrorResponse {
    /// <summary>	A machine-readable error code. This is unique for each error scenario.</summary>
    public string Code { get; set; }

    /// <summary>	A human-readable explanation for the error.</summary>
    public string Message { get; set; }
  }
}
