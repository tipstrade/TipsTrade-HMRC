using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT payments.</summary>
  public class PaymentsRequest : DateRangeRequest, IApiRequest {
    /// <summary>Returns multiple valid payments when used with dates from 2017-02-27 and to 2017-12-21.</summary>
    [GovTestScenario]
    public const string ScenarioMultiplePayment = "MULTIPLE_PAYMENTS";

    /// <summary>Returns a single valid payment when used with dates from 2017-01-02 and to 2017-02-02.</summary>
    [GovTestScenario]
    public const string ScenarioSinglePayment = "SINGLE_PAYMENT";

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => $"{Vrn}/payments";

    void IApiRequest.PopulateRequest(IRestRequest request) {
    }
  }
}
