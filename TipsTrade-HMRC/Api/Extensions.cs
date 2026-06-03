using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>
  /// Provides extension methods for working with HMRC API requests and responses, including adding common parameters, handling responses, and extracting information from request types.
  /// </summary>
  public static class Extensions {
    /// <summary>
    /// Add the date range parameters ("from" and "to") to the specified <see cref="RestRequest"/>.
    /// </summary>
    /// <param name="request">The <see cref="RestRequest"/> to augment.</param>
    /// <param name="range">The date range to translate into query parameters.</param>
    /// <param name="type">The <see cref="ParameterType"/> to use when adding parameters (defaults to <see cref="ParameterType.QueryString"/>).</param>
    /// <returns>The same <see cref="RestRequest"/> instance to allow fluent usage.</returns>
    internal static RestRequest AddDateRangeParameters(this RestRequest request, IDateRange range, ParameterType type = ParameterType.QueryString) {
      if (range.DateFrom != default) {
        request.AddParameter("from", $"{range.DateFrom:yyyy-MM-dd}", type);
      }
      if (range.DateTo != default) {
        request.AddParameter("to", $"{range.DateTo:yyyy-MM-dd}", type);
      }

      return request;
    }

    /// <summary>
    /// Add the sandbox-only "Gov-Test-Scenario" header to the specified <see cref="RestRequest"/> when a gov test scenario is set.
    /// </summary>
    /// <param name="request">The <see cref="RestRequest"/> to augment.</param>
    /// <param name="scenario">The <see cref="IGovTestScenario"/> containing the scenario value.</param>
    /// <returns>The same <see cref="RestRequest"/> instance to allow fluent usage.</returns>
    internal static RestRequest AddGovTestScenario(this RestRequest request, IGovTestScenario scenario) {
      if (scenario.GovTestScenario != null && !string.IsNullOrEmpty(scenario.GovTestScenario)) {
        request.AddHeader("Gov-Test-Scenario", scenario.GovTestScenario);
      }

      return request;
    }

    /// <summary>
    /// Retrieves all service name constants declared on the runtime type of the supplied
    /// <see cref="ICreateTestUserRequest"/> that are decorated with <see cref="ServiceNameAttribute"/>.
    /// </summary>
    /// <param name="request">The create-test-user request whose concrete type is inspected for service name fields.</param>
    /// <returns>
    /// An <see cref="IEnumerable{String}"/> containing the string values of all public static fields on the
    /// request's type that have <see cref="ServiceNameAttribute"/> applied. If no such fields are present
    /// the returned sequence will be empty.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> is <c>null</c>.</exception>
    public static IEnumerable<string> GetServiceNames(this ICreateTestUserRequest request) {
      if (request == null) {
        throw new ArgumentNullException(nameof(request));
      }

      return request.GetType()
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.GetCustomAttribute<ServiceNameAttribute>() != null)
        .Select(f => f.GetValue(null))
        .OfType<string>();
    }

    /// <summary>
    /// Inspect a <see cref="RestResponse{T}"/>, throw on HTTP errors and translate the response into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="response">The <see cref="RestResponse{T}"/> returned from the <see cref="RestClient"/> execution.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> to log any relevant information during response handling.</param>
    /// <returns>An instance of <typeparamref name="T"/> populated from the HTTP response body and selected headers.</returns>
    internal static T HandleResponse<T>(this RestResponse<T> response, ILogger? logger = null) where T : class, new() {
      response.ThrowOnError();

      // ThrowOnError ensures the response is successful, but the body may still be empty (e.g. 204 No Content). In that case we want to return a new instance of T rather than null.
      var data = response.Data ?? new T();

      if (data is ICorrelationId correlation && response.TryGetGuidFromHeader(logger, "X-CorrelationId", out var guid)) {
        correlation.CorrelationId = guid;
      }

      if (data is IDeprecationDate deprecation && response.TryGetRequiredHeaderValue(logger, "Deprecation", out var deprecationDate)) {
        deprecation.DeprecationDate = deprecationDate;
      }

      if (data is ISunsetDate sunset && response.TryGetRequiredHeaderValue(logger, "Sunset", out var sunsetDate)) {
        sunset.SunsetDate = sunsetDate;
      }

      if (data is IDocumentationLink documentation && response.TryGetRequiredHeaderValue(logger, "Link", out var documentationLink)) {
        documentation.DocumentationLink = documentationLink;
      }

      if (data is IReceipt receipt) {
        if (response.TryGetRequiredHeaderValue(logger, "Receipt-ID", out var id)) {
          receipt.ReceiptID = Guid.Parse(id);
        }

        if (response.TryGetDateTimeFromHeader(logger, "Receipt-Timestamp", out var timestamp)) {
          receipt.ReceiptTimestamp = timestamp;
        }
      }

      return data;
    }

    /// <summary>
    /// Throws an <see cref="ApiException"/> if the HTTP response indicates a failure.
    /// </summary>
    /// <param name="response">The <see cref="RestResponse"/> to examine for errors.</param>
    /// <exception cref="ApiException">Thrown when the RestResponse was not successful. The exception includes the parsed <see cref="ErrorResponse"/> when available.</exception>
    internal static void ThrowOnError(this RestResponse response) {
      if (response.IsSuccessful) {
        return;
      }

      int code = (int)response.StatusCode;
      ErrorResponse? error = null;

      try {
        if (response.Content != null) {
          error = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
        }
      } catch { }

      throw new ApiException(error?.Message ?? response?.StatusDescription ?? "", response?.ErrorException) {
        Status = response?.StatusCode,
        ApiError = error,
        Data = {
          { "Request", response?.Request },
          { "Response", response }
        }
      };
    }

    /// <summary>
    /// Attempts to retrieve the value of a required header from the <see cref="RestResponse"/>. Logs a warning if the header is missing.
    /// </summary>
    /// <param name="response">The <see cref="RestResponse"/> containing the headers.</param>
    /// <param name="logger">The <see cref="ILogger"/> to use for logging warnings.</param>
    /// <param name="headerName">The name of the header to retrieve.</param>
    /// <param name="value">The retrieved header value if successful, otherwise an empty string.</param>
    /// <returns>True if the header is present and contains a non-null value, false otherwise.</returns>
    internal static bool TryGetRequiredHeaderValue(this RestResponse response, ILogger? logger, string headerName, out string value) {
      var found = response.Headers?.Where(h => headerName.Equals(h.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value;

      if (found == null) {
        logger?.LogWarning("Response is missing expected {HeaderName} header.", headerName);
        value = string.Empty;
        return false;
      }

      value = found;
      return true;
    }

    /// <summary>
    /// Attempts to parse a GUID value from the specified header in the <see cref="RestResponse"/>.
    /// </summary>
    /// <param name="response">The <see cref="RestResponse"/> containing the headers.</param>
    /// <param name="logger">The <see cref="ILogger"/> to use for logging warnings.</param>
    /// <param name="headerName">The name of the header to parse.</param>
    /// <param name="guid">The parsed GUID value if successful, otherwise <see cref="Guid.Empty"/>.</param>
    /// <returns>True if the header is present and contains a valid GUID, false otherwise.</returns>
    internal static bool TryGetGuidFromHeader(this RestResponse response, ILogger? logger, string headerName, out Guid guid) {
      guid = Guid.Empty;

      if (!response.TryGetRequiredHeaderValue(logger, headerName, out var id)) {
        return false;
      }

      if (Guid.TryParse(id, out guid)) {
        return true;
      }

      logger?.LogWarning("Response has invalid {HeaderName} header value: {Value}", headerName, id);
      return false;
    }

    /// <summary>
    /// Attempts to parse a DateTime value from the specified header in the <see cref="RestResponse"/>.
    /// </summary>
    /// <param name="response">The <see cref="RestResponse"/> containing the headers.</param>
    /// <param name="logger">The <see cref="ILogger"/> to use for logging warnings.</param>
    /// <param name="headerName">The name of the header to parse.</param>
    /// <param name="dateTime">The parsed DateTime value if successful, otherwise <see cref="DateTime.MinValue"/>.</param>
    /// <returns>True if the header is present and contains a valid DateTime, false otherwise.</returns>
    internal static bool TryGetDateTimeFromHeader(this RestResponse response, ILogger? logger, string headerName, out DateTime dateTime) {
      dateTime = DateTime.MinValue;

      if (!response.TryGetRequiredHeaderValue(logger, headerName, out var value)) {
        return false;
      }

      if (DateTime.TryParse(value.ToString(), out dateTime)) {
        return true;
      }

      logger?.LogWarning("Response has invalid {HeaderName} header value: {Value}", headerName, value);
      return false;
    }
  }
}
