using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT liabilities.</summary>
  public class LiabilitiesRequest : DateRangeRequest, IApiRequest {
    /// <summary>Returns multiple valid liabilities when used with dates from 2017-04-05 and to 2017-12-21.</summary>
    [GovTestScenario]
    public const string ScenarioMultipleLiabilities = "MULTIPLE_LIABILITIES";

    /// <summary>Returns a single valid liability when used with dates from 2017-01-02 and to 2017-02-02.</summary>
    [GovTestScenario]
    public const string ScenarioSingleLiability = "SINGLE_LIABILITY";

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => $"{Vrn}/liabilities";

    void IApiRequest.PopulateRequest(IRestRequest request) {
      request.AddJsonBodyNewtonsoft(this);
    }
  }
}
