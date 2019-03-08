namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a response containing error information.</summary>
  public class ErrorResponse {
    #region Properties
    /// <summary>A machine-readable error code. This is unique for each error scenario.</summary>
    public string Code { get; set; }

    /// <summary>A list of errors that occurred.</summary>
    public ErrorResponse[] Errors { get; set; }

    /// <summary>A human-readable explanation for the error.</summary>
    public string Message { get; set; }

    /// <summary>The path that caused the error to occur.</summary>
    public string Path { get; set; }
    #endregion

    #region Methods
    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"{Message}";
    }
    #endregion
  }
}
