using RestSharp;
using System;
using System.Net.Http;
using TipsTrade.HMRC.Extensions;

namespace TipsTrade.HMRC.Api {
  internal interface IHmrcRestClient {
    HmrcOptions Options { get; }
    Lazy<RestClient> RestClient { get; }
  }

  internal static class HmrcRestClientExtensions {
    internal static Lazy<RestClient> BuildRestClient(this IHmrcRestClient client, IHttpClientFactory httpClientFactory) {
      if (httpClientFactory == null) {
        throw new ArgumentNullException(nameof(httpClientFactory));
      }

      return new Lazy<RestClient>(() => {
        var httpClient = httpClientFactory.CreateClient(ServiceCollectionExtensions.HttpClientName);
        return new RestClient(httpClient, new RestClientOptions(client.Options.BaseUrl));
      });
    }

    internal static HmrcOptions GetOptions(this IHmrcRestClient client) => client.Options;

    internal static RestClient GetRestClient(this IHmrcRestClient client) => client.RestClient.Value;
  }
}
