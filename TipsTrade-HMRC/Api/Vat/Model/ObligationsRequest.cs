using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT obligations.</summary>
  public class ObligationsRequest : DateRangeRequest, IApiRequest {
    /// <summary>Which obligation statuses to return (O=Open, F=Fulfilled).</summary>
    public ObligationStatus? Status { get; set; }

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => $"{Vrn}/obligations";

    void IApiRequest.PopulateRequest(IRestRequest request) {
      if (Status != null) {
        request.AddParameter("status", $"{Status}"[0]);
      }
    }
  }
}
