using RestSharp;
using System.Net.Http;

namespace TipsTrade.HMRC.Extensions {
  /// <summary>
  /// Provides extension methods for the IHttpClientFactory to create HttpClient instances configured for the HMRC API.
  /// </summary>
  internal static class HttpClientFactoryExtensions {
    internal static RestClient CreateHmrcRestClient(this IHttpClientFactory factory) {
      var client = factory.CreateClient(ServiceCollectionExtensions.HttpClientName);

      return new RestClient(client, disposeHttpClient: false);
    }
  }
}
