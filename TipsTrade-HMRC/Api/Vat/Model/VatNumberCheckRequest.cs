using RestSharp;
using System.Web;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  internal class VerifiedVatNumberCheckRequest : VatNumberCheckRequest {
    protected override string GetLocation() {
      return $"lookup/{HttpUtility.UrlEncode(Vrn)}/{HttpUtility.UrlEncode(RequesterVrn)}";
    }

    public string RequesterVrn { get; set; }
  }

  internal class VatNumberCheckRequest : IApiRequest {
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.Application;

    string IApiRequest.ContentType => null;

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => GetLocation();

    void IApiRequest.PopulateRequest(IRestRequest request) {
    }

    public string Vrn { get; set; }

    protected virtual string GetLocation() {
      return $"lookup/{HttpUtility.UrlEncode(Vrn)}";
    }
  }
}
