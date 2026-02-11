using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>A collection of methods extending the functionality of TipsTrade.HMRC.Api.IApi objects.</summary>
  public static class Extensions {
    /// <summary>The default content type to be expected.</summary>
    private const string DefaultContentType = "json";

    /// <summary>Add the date range parameters to the specified request.</summary>
    internal static RestRequest AddDateRangeParameters(this RestRequest request, IDateRange range, ParameterType type = ParameterType.QueryString) {
      if (range.DateFrom != default(DateTime)) {
        request.AddParameter("from", $"{range.DateFrom:yyyy-MM-dd}", type);
      }
      if (range.DateTo != default(DateTime)) {
        request.AddParameter("to", $"{range.DateTo:yyyy-MM-dd}", type);
      }

      return request;
    }

    /// <summary>Add the Gov-Test-Scenario header to the specified request.</summary>
    internal static RestRequest AddGovTestScenario(this RestRequest request, IGovTestScenario scenario) {
      if (!string.IsNullOrEmpty(scenario.GovTestScenario)) {
        request.AddHeader("Gov-Test-Scenario", scenario.GovTestScenario);
      }

      return request;
    }

    internal static RestRequest CreateRequest(this IApi api, IApiRequest request) {
      var client = api.GetClient();

      var restRequest = new RestRequest($"{api.Location}/{request.Location}", request.Method);
      restRequest.AddHeader("Accept", api.GetAcceptHeader(request.AcceptType));

      if (!string.IsNullOrEmpty(request.ContentType)) {
        restRequest.AddHeader("Content-Type", request.ContentType);
      }

      if (client.IsSandbox && request is IGovTestScenario govTest) {
        restRequest.AddGovTestScenario(govTest);
      }

      if (request is IDateRange dateRange) {
        restRequest.AddDateRangeParameters(dateRange);
      }

      if (request.Authorization == Authorization.Application) {
        if (string.IsNullOrEmpty(client.ServerToken))
          throw new InvalidOperationException($"The {nameof(client.ServerToken)} cannot be empty");

        restRequest.AddHeader("Authorization", $"Bearer {client.ServerToken}");

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

    /// <summary>Executes the specified request for the API.</summary>
    internal static T ExecuteRequest<T>(this IApi api, RestRequest request) {
      var client = api.GetRestClient();
      var response = client.Execute<T>(request);
      response.ThrowOnError();

      var data = response.Data;

      if (typeof(ICorrelationId).IsAssignableFrom(typeof(T))) {
        var id = response.Headers.Where(h => "X-CorrelationId".Equals(h.Name, StringComparison.CurrentCultureIgnoreCase)).First().Value;
        ((ICorrelationId)data).CorrelationId = Guid.Parse($"{id}");
      }

      if (typeof(IReceipt).IsAssignableFrom(typeof(T))) {
        var id = response.Headers.Where(h => "Receipt-ID".Equals(h.Name, StringComparison.CurrentCultureIgnoreCase)).First().Value;
        var timestamp = response.Headers.Where(h => "Receipt-Timestamp".Equals(h.Name, StringComparison.CurrentCultureIgnoreCase)).First().Value;

        var receipt = data as IReceipt;
        receipt.ReceiptID = Guid.Parse($"{id}");
        receipt.ReceiptTimestamp = DateTime.Parse($"{timestamp}");
      }

      return data;
    }

    /// <summary>
    /// Gets the version header required by the HMRC Api.
    /// <see cref="!:https://developer.service.hmrc.gov.uk/api-documentation/docs/reference-guide#versioning"/>
    /// </summary>
    /// <param name="api">The Api for which the header should be generated</param>
    /// <param name="contentType">The optional content type to be accepted.</param>
    /// <returns>A string containing a valide HTTP Accept header.</returns>
    internal static string GetAcceptHeader(this IApi api, string contentType = DefaultContentType) {
      return $"application/vnd.hmrc.{api.Version}+{contentType}";
    }

    /// <summary>Gets the HMRC client for the specified API.</summary>
    internal static Client GetClient(this IApi api) {
      if (!(api is IClient)) {
        throw new InvalidOperationException($"{nameof(api)} does not implement {typeof(IClient)}");
      }

      return ((IClient)api).Client;
    }

    /// <summary>Gets the rest client for the specified API.</summary>
    internal static RestClient GetRestClient(this IApi api) {
      return new RestClient(api.GetClient().BaseUrl);
    }

    /// <summary>Gets all the service names for the current ICreateTestUserRequest.</summary>
    public static IEnumerable<string> GetServiceNames(this ICreateTestUserRequest request) {
      return request.GetType()
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.GetCustomAttribute<ServiceNameAttribute>() != null)
        .Select(f => (string)f.GetValue(null));
    }

    /// <summary>Throws an ApiException if an error is encountered.</summary>
    /// <param name="response"></param>
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
