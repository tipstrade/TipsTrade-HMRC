using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT payments.</summary>
  public class PaymentsRequest : DateRangeRequest, IApiRequest {
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => $"{Vrn}/payments";

    void IApiRequest.PopulateRequest(IRestRequest request) {
    }
  }
}
