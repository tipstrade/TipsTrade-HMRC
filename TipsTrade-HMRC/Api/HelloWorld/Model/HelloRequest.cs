using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.HelloWorld.Model {
  internal class HelloRequest : IApiRequest {
    private readonly string location;
    private readonly Authorization authorization;

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => authorization;

    string IApiRequest.ContentType => null;

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => location;

    void IApiRequest.PopulateRequest(IRestRequest request) {
    }

    internal HelloRequest(string location, Authorization authorization) {
      this.authorization = authorization;
      this.location = location;
    }
  }
}
