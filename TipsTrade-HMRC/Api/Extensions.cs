using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>
  /// A collection of methods extending the functionality of <see cref="IApi"/> objects.
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
      if (!string.IsNullOrEmpty(scenario.GovTestScenario)) {
        request.AddHeader("Gov-Test-Scenario", scenario.GovTestScenario);
      }

      return request;
    }

    /// <summary>
    /// Create and populate a <see cref="RestRequest"/> from a given <see cref="IApiRequest"/> using API client settings.
    /// </summary>
    /// <param name="api">The API instance used to determine client settings and endpoints.</param>
    /// <param name="request">The request model that will populate headers, body and parameters.</param>
    /// <returns>A fully populated <see cref="RestRequest"/> ready for execution.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when required tokens (server or user) are missing for the requested <see cref="Authorization"/> mode,
    /// or when anti-fraud headers are required but the client's <see cref="AntiFraud"/> instance is null.
    /// </exception>
    internal static RestRequest CreateRequest(this IApi api, IApiRequest request) {
      var client = api.GetClient();

      var restRequest = new RestRequest($"{api.Location}/{request.Location}", request.Method);
      restRequest.AddHeader("Accept", api.GetAcceptHeader(request.AcceptType));

      // Can only have a content type header if there is a body
      var canHaveContentType = request.Method == Method.Post || request.Method == Method.Put || request.Method == Method.Patch;
      if (!string.IsNullOrEmpty(request.ContentType) && canHaveContentType) {
        restRequest.AddHeader("Content-Type", request.ContentType);
      }

      if (client.IsSandbox && request is IGovTestScenario govTest) {
        restRequest.AddGovTestScenario(govTest);
      }

      if (request is IDateRange dateRange) {
        restRequest.AddDateRangeParameters(dateRange);
      }

      if (request.Authorization == Authorization.Application) {
        var token = client.GetApplicationToken();

        // GetApplicationToken will throw if the client credentials aren't valid, so we can assume that the access token is valid.
        restRequest.AddHeader("Authorization", $"Bearer {token.AccessToken}");

      } else if (request.Authorization == Authorization.User) {
        if (string.IsNullOrEmpty(client.AccessToken))
          throw new InvalidOperationException($"The {nameof(client.AccessToken)} cannot be empty");

        restRequest.AddHeader("Authorization", $"Bearer {client.AccessToken}");

      }

      if (api is IRequiresAntiFraud) {
        if (client.AntiFraud == null) throw new InvalidOperationException($"The {api.Name} requires Anti Fraud headers.");
        foreach (var item in client.AntiFraud.GetAntiFraudHeaders()) {
          restRequest.AddHeader(item.Key, item.Value);
        }
      }

      request.PopulateRequest(restRequest);

      return restRequest;
    }

    /// <summary>
    /// Create a <see cref="RestRequest"/> from the supplied <see cref="IApiRequest"/> and execute it synchronously,
    /// deserializing the response into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="api">The API instance used to execute the request.</param>
    /// <param name="request">The request model used to create the HTTP request.</param>
    /// <returns>An instance of <typeparamref name="T"/> representing the API response.</returns>
    internal static T ExecuteRequest<T>(this IApi api, IApiRequest request) {
      var restRequest = api.CreateRequest(request);

      return api.ExecuteRequest<T>(restRequest);
    }

    /// <summary>
    /// Execute the specified <see cref="RestRequest"/> synchronously and handle the response.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="api">The API instance used to obtain the <see cref="RestClient"/> for execution.</param>
    /// <param name="request">The <see cref="RestRequest"/> to execute.</param>
    /// <returns>An instance of <typeparamref name="T"/> representing the API response.</returns>
    internal static T ExecuteRequest<T>(this IApi api, RestRequest request) {
      var client = api.GetRestClient();
      var response = client.Execute<T>(request);

      return HandleResponse(response);
    }

    /// <summary>
    /// Create a <see cref="RestRequest"/> from the supplied <see cref="IApiRequest"/> and execute it asynchronously,
    /// deserializing the response into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="api">The API instance used to execute the request.</param>
    /// <param name="request">The request model used to create the HTTP request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the async operation.</param>
    /// <returns>A task that resolves to an instance of <typeparamref name="T"/> representing the API response.</returns>
    internal static async Task<T> ExecuteRequestAsync<T>(this IApi api, IApiRequest request, CancellationToken cancellationToken) {
      var restRequest = api.CreateRequest(request);

      return await api.ExecuteRequestAsync<T>(restRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Execute the specified <see cref="RestRequest"/> asynchronously and handle the response.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="api">The API instance used to obtain the <see cref="RestClient"/> for execution.</param>
    /// <param name="request">The <see cref="RestRequest"/> to execute.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the async operation.</param>
    /// <returns>A task that resolves to an instance of <typeparamref name="T"/> representing the API response.</returns>
    internal static async Task<T> ExecuteRequestAsync<T>(this IApi api, RestRequest request, CancellationToken cancellationToken) {
      var client = api.GetRestClient();
      var response = await client.ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);

      return HandleResponse(response);
    }

    /// <summary>
    /// Gets the versioned Accept header required by the HMRC API.
    /// </summary>
    /// <param name="api">The API for which the header should be generated.</param>
    /// <param name="contentType">The optional content type to be accepted (usually json).</param>
    /// <returns>A string containing a valid HTTP Accept header for the HMRC API versioning scheme.</returns>
    /// <remarks>
    /// See HMRC API versioning guidance: <see href="https://developer.service.hmrc.gov.uk/api-documentation/docs/reference-guide#versioning" />
    /// </remarks>
    internal static string GetAcceptHeader(this IApi api, string contentType) {
      return $"application/vnd.hmrc.{api.Version}+{contentType}";
    }

    /// <summary>
    /// Gets the HMRC <see cref="Client"/> instance associated with the specified <see cref="IApi"/>.
    /// </summary>
    /// <param name="api">The API instance that must also implement <see cref="IClient"/>.</param>
    /// <returns>The <see cref="Client"/> associated with <paramref name="api"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="api"/> does not implement <see cref="IClient"/>.</exception>
    internal static Client GetClient(this IApi api) {
      if (!(api is IClient)) {
        throw new InvalidOperationException($"{nameof(api)} does not implement {typeof(IClient)}");
      }

      return ((IClient)api).Client;
    }

    /// <summary>
    /// Construct a new <see cref="RestClient"/> using the <see cref="Client.BaseUrl"/> of the specified API's client.
    /// </summary>
    /// <param name="api">The API instance used to obtain the base URL.</param>
    /// <returns>A new <see cref="RestClient"/> configured with the client's base URL.</returns>
    internal static RestClient GetRestClient(this IApi api) {
      return new RestClient(api.GetClient().BaseUrl);
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
        .Select(f => (string)f.GetValue(null));
    }

    /// <summary>
    /// Inspect a <see cref="RestResponse{T}"/>, throw on HTTP errors and translate the response into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="response">The <see cref="RestResponse{T}"/> returned from the <see cref="RestClient"/> execution.</param>
    /// <returns>An instance of <typeparamref name="T"/> populated from the HTTP response body and selected headers.</returns>
    private static T HandleResponse<T>(RestResponse<T> response) {
      response.ThrowOnError();

      // Some endpoints return 204 No Content with an empty body, in which case we should return an empty instance of T instead of trying to deserialize the empty body.
      var data = response.StatusCode == System.Net.HttpStatusCode.NoContent ?
        Activator.CreateInstance<T>()
        : response.Data;

      if (data is ICorrelationId correlation) {
        var id = response.Headers.Where(h => "X-CorrelationId".Equals(h.Name, StringComparison.OrdinalIgnoreCase)).First().Value;

        correlation.CorrelationId = Guid.Parse(id);
      }

      if (data is IDeprecationDate deprecation) {
        deprecation.DeprecationDate = response.Headers.Where(h => "Deprecation".Equals(h.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value;
      }

      if (data is ISunsetDate sunset) {
        sunset.SunsetDate = response.Headers.Where(h => "Sunset".Equals(h.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value;
      }

      if (data is IDocumentationLink documentation) {
        documentation.DocumentationLink = response.Headers.Where(h => "Link".Equals(h.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value;
      }

      if (data is IReceipt receipt) {
        var id = response.Headers.Where(h => "Receipt-ID".Equals(h.Name, StringComparison.OrdinalIgnoreCase)).First().Value;
        var timestamp = response.Headers.Where(h => "Receipt-Timestamp".Equals(h.Name, StringComparison.OrdinalIgnoreCase)).First().Value;

        receipt.ReceiptID = Guid.Parse(id);
        receipt.ReceiptTimestamp = DateTime.Parse(timestamp);
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
      ErrorResponse error = null;

      try {
        error = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
      } catch { }

      throw new ApiException(error?.Message ?? response.StatusDescription, response.ErrorException) {
        Status = response.StatusCode,
        ApiError = error
      };
    }
  }
}
