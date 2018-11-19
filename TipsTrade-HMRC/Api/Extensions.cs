using Newtonsoft.Json;
using RestSharp;
using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>A collection of methods extending the functionality of TipsTrade.HMRC.Api.IApi objects.</summary>
  public static class Extensions {
    /// <summary>The default content type to be expected.</summary>
    private const string DefaultContentType = "json";

    /// <summary>Json content type.</summary>
    private const string JsonContentType = "application/json";

    /// <summary>Shorthand method for serializing the body using Newtonsoft.Json.</summary>
    internal static void AddJsonBodyNewtonsoft(this IRestRequest request, object value) {
      request.IsJsonContent();
      request.RequestFormat = DataFormat.Json;
      request.AddParameter(request.JsonSerializer.ContentType, JsonConvert.SerializeObject(value), ParameterType.RequestBody);
    }

    /// <summary>Add the date range parameters to the specified request.</summary>
    internal static IRestRequest AddDateRangeParameters(this IRestRequest request, IDateRange range, ParameterType type = ParameterType.QueryString) {
      request.AddParameter("from", $"{range.From:yyyy-MM-dd}", type);
      request.AddParameter("to", $"{range.To:yyyy-MM-dd}", type);

      return request;
    }

    /// <summary>Creates a api request for the specified method.</summary>
    internal static RestRequest CreateRequest(this IApi api, string methodName, Method method = Method.GET, Authorization authorization = Authorization.Open, string contentType = DefaultContentType) {
      var client = api.GetClient();

      var req = new RestRequest($"{api.Location}/{methodName}", method);
      req.AddHeader("Accept", api.GetAcceptHeader());

      if (authorization == Authorization.Application) {
        req.AddHeader("Authorization", $"Bearer {client.ServerToken}");
      } else if (authorization == Authorization.User) {
        req.AddHeader("Authorization", $"Bearer {client.AccessToken}");
      }

      return req;
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

      return response.DeserializeContent<T>();
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
          ErrorCode = errorResponse.Code,
          ErrorMessage = errorResponse.Message
        };
      }

    }
  }
}
