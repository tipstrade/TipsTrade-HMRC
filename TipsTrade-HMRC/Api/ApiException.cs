using System;
using System.Net;

namespace TipsTrade.HMRC.Api {
  /// <summary>Represents errors that are thrown by the HMRC API.</summary>
  public class ApiException : Exception {
    /// <summary>A code indicating the error that occurred.</summary>
    public string ErrorCode { get; set; }

    /// <summary>A message describing the error that occurred.</summary>
    public string ErrorMessage { get; set; }

    /// <summary>The HTTP Status Code that was returned.</summary>
    public HttpStatusCode Status { get; internal set; }

    /// <summary>Initializes a new instance of the TipsTrade.HMRC.Api.ApiException class.</summary>
    public ApiException() {
    }

    /// <summary>Initializes a new instance of the TipsTrade.HMRC.Api.ApiException class with a specified error message.</summary>
    public ApiException(string message) : base(message) {
    }

    /// <summary>Initializes a new instance of the TipsTrade.HMRC.Api.ApiException class with a specified error message.</summary>
    public ApiException(string message, Exception innerException) : base(message, innerException) {
    }
  }
}
