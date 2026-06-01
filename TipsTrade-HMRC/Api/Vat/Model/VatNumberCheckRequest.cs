using RestSharp;
using System.Web;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  internal class VerifiedVatNumberCheckRequest : VatNumberCheckRequest {
    protected override string GetLocation() {
      return $"lookup/{HttpUtility.UrlEncode(Vrn)}/{HttpUtility.UrlEncode(RequesterVrn)}";
    }

    public string RequesterVrn { get; set; } = "";
  }

  internal class VatNumberCheckRequest : IApiRequest {
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.Application;

    Method IApiRequest.Method => Method.Get;

    string IApiRequest.Location => GetLocation();

    public string Vrn { get; set; } = "";

    protected virtual string GetLocation() {
      return $"lookup/{HttpUtility.UrlEncode(Vrn)}";
    }
  }
}
