using System;
using System.Net;

namespace TipsTrade.HMRC.Api {
  /// <summary>Represents errors that are thrown by the HMRC API.</summary>
  public class ApiException : Exception {
    /// <summary>A code indicating the error that occurred.</summary>
    public string ErrorCode { get; set; }

    /// <summary>A message describing the error that occurred.</summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// A flag indicating whether the exception was 404 - NOT_FOUND.
    /// Needed as some APIs return this for empty collections, not to be confused with 404 - MATCHING_RESOURCE_NOT_FOUND.
    /// </summary>
    public bool IsNotFound => "NOT_FOUND".Equals(ErrorCode);

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
