using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  internal class ValidateRequest : IApiRequest {
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.Application;

    string IApiRequest.ContentType => null;

    Method IApiRequest.Method => Method.Get;

    string IApiRequest.Location => "validate";

    void IApiRequest.PopulateRequest(RestRequest request) {
    }
  }
}
