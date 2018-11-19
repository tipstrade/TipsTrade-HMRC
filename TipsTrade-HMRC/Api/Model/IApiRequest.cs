using RestSharp;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that all Api requests must inherit from.</summary>
  internal interface IApiRequest {
    string AcceptType { get; }

    Authorization Authorization { get; }

    string ContentType { get; }

    Method Method { get; }

    string Location { get; }

    void PopulateRequest(IRestRequest request);
  }
}
