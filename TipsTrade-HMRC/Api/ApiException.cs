using RestSharp;
using System;
using System.Linq;
using System.Net;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>Represents errors that are thrown by the HMRC API.</summary>
  /// <remarks>
  /// The Data property of the exception may contain additional information about the error, such as the API error code and message, and
  /// HTTP request and response details. As such it should be treated as sensitive and not logged or exposed to end users.
  /// </remarks>
  public class ApiException : Exception {
    /// <summary>The API error that caused to exception to be thrown.</summary>
    public ErrorResponse? ApiError { get; set; }

    /// <summary>A flag indicating whether the credentials are invalid.</summary>
    [Obsolete("This was unreliably set based on the API error code.")]
    public bool IsInvalidCredentials => "INVALID_CREDENTIALS".Equals(ApiError?.Code);

    /// <summary>
    /// A flag indicating whether the exception was 404 - NOT_FOUND.
    /// Needed as some APIs return this for empty collections, not to be confused with 404 - MATCHING_RESOURCE_NOT_FOUND.
    /// </summary>
    [Obsolete("This was unreliably set based on the API error code.")]
    public bool IsNotFound => "NOT_FOUND".Equals(ApiError?.Code);

    /// <summary>The HTTP Status Code that was returned.</summary>
    public HttpStatusCode? Status { get; internal set; }

    /// <summary>Initializes a new instance of the TipsTrade.HMRC.Api.ApiException class.</summary>
    public ApiException() {
    }

    /// <summary>Initializes a new instance of the TipsTrade.HMRC.Api.ApiException class with a specified error message.</summary>
    public ApiException(string message) : base(message) {
    }

    /// <summary>Initializes a new instance of the TipsTrade.HMRC.Api.ApiException class with a specified error message.</summary>
    public ApiException(string message, Exception? innerException) : base(message, innerException) {
    }
  }

  internal static class ApiExceptionExtensions {
    public static ApiException AddApiError(this ApiException exception, ErrorResponse? apiError) {
      if (exception == null) {
        throw new ArgumentNullException(nameof(exception));
      }

      exception.ApiError = apiError;

      return exception;
    }

    public static ApiException AddRequestData(this ApiException exception, RestRequest? request) {
      if (exception == null) {
        throw new ArgumentNullException(nameof(exception));
      }

      if (request != null) {
        exception.Data.Add("RequestMethod", request.Method);
        exception.Data.Add("RequestUri", request.Resource);
        exception.Data.Add("ParameterNames", string.Join(", ", request.Parameters.Select(x => x.Name)));
      }

      return exception;
    }

    public static ApiException AddResponseData(this ApiException exception, RestResponse? response) {
      if (exception == null) {
        throw new ArgumentNullException(nameof(exception));
      }

      if (response != null) {
        if (exception.Status == null) {
          exception.Status = response.StatusCode;
        }

        exception.Data.Add("ResponseContent", response.Content);
      }

      return exception.AddRequestData(response?.Request);
    }

    public static ApiException AddTenantId(this ApiException exception, string? tenantId) {
      if (exception == null) {
        throw new ArgumentNullException(nameof(exception));
      }

      if (tenantId != null) {
        exception.Data.Add("TenantId", tenantId);
      }

      return exception;
    }
  }
}
