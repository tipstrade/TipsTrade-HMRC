using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>A collection of methods extending the functionality of TipsTrade.HMRC.Api.IApi objects.</summary>
  public static class Extensions {
    /// <summary>The default content type to be expected.</summary>
    private const string DefaultContentType = "json";

    /// <summary>Json content type.</summary>
    private const string JsonContentType = "application/json";

    /// <summary>Shorthand method for serializing the body using Newtonsoft.Json.</summary>
    internal static IRestRequest AddJsonBodyNewtonsoft(this IRestRequest request, object value) {
      request.IsJsonContent();
      request.RequestFormat = DataFormat.Json;
      request.AddParameter(request.JsonSerializer.ContentType, JsonConvert.SerializeObject(value), ParameterType.RequestBody);

      return request;
    }

    /// <summary>Add the date range parameters to the specified request.</summary>
    internal static IRestRequest AddDateRangeParameters(this IRestRequest request, IDateRange range, ParameterType type = ParameterType.QueryString) {
      request.AddParameter("from", $"{range.From:yyyy-MM-dd}", type);
      request.AddParameter("to", $"{range.To:yyyy-MM-dd}", type);

      return request;
    }

    /// <summary>Add the Gov-Test-Scenario header to the specified request.</summary>
    internal static IRestRequest AddGovTestScenario(this IRestRequest request, IGovTestScenario scenario) {
      if (!string.IsNullOrEmpty(scenario.GovTestScenario)) {
        request.AddHeader("Gov-Test-Scenario", scenario.GovTestScenario);
      }

      return request;
    }

    internal static IRestRequest CreateRequest(this IApi api, IApiRequest request) {
      var client = api.GetClient();

      var restRequest = new RestRequest($"{api.Location}/{request.Location}", request.Method);
      restRequest.AddHeader("Accept", api.GetAcceptHeader(request.AcceptType));

      if (!string.IsNullOrEmpty(request.ContentType)) {
        restRequest.AddHeader("Content-Type", request.ContentType);
      }

      if (request is IGovTestScenario) {
        restRequest.AddGovTestScenario(request as IGovTestScenario);
      }

      if (request is IDateRange) {
        restRequest.AddDateRangeParameters(request as IDateRange);
      }

      if (request.Authorization == Authorization.Application) {
        restRequest.AddHeader("Authorization", $"Bearer {client.ServerToken}");
      } else if (request.Authorization == Authorization.User) {
        restRequest.AddHeader("Authorization", $"Bearer {client.AccessToken}");
      }

      request.PopulateRequest(restRequest);

      return restRequest;
    }

    /// <summary>Deserializes the content.</summary>
    internal static T DeserializeContent<T>(this IRestResponse response) {
      response.ThrowOnError();
      return JsonConvert.DeserializeObject<T>(response.Content);
    }

    /// <summary>Executes the specified request for the API.</summary>
    internal static T ExecuteRequest<T>(this IApi api, IRestRequest request) {
      var client = api.GetRestClient();
      var response =  client.Execute(request);
      response.ThrowOnError();

      var data = response.DeserializeContent<T>();

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
      if (! (api is IClient)) {
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

    /// <summary>Sets the Content-Type of the request to application/json</summary>
    internal static IRestRequest IsJsonContent(this IRestRequest request) {
      request.AddHeader("Content-Type", JsonContentType);
      return request;
    }

    /// <summary>Throws an ApiException if an error is encountered.</summary>
    /// <param name="response"></param>
    internal static void ThrowOnError(this IRestResponse response) {
      int code = (int)response.StatusCode;

      if (code >= 400 && code <= 599) {
        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
        throw new ApiException(errorResponse.Message, response.ErrorException) {
          ApiError = errorResponse
        };
      }

    }
  }
}
