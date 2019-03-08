using Newtonsoft.Json;
using RestSharp;
using System.Web;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve a VAT return.</summary>
  public class ReturnRequest : IApiRequest, IVatRequest {
    /// <summary>The date of the requested return cannot be further than four years from the current date.</summary>
    [GovTestScenario(errorCode: "DATE_RANGE_TOO_LARGE")]
    public const string ScenarioDateRangeTooLarge = "DATE_RANGE_TOO_LARGE";

    /// <summary>The ID code for the period that this obligation belongs to.
    /// The format is a string of four alphanumeric characters.
    /// Occasionally the format includes the “#” symbol, which must be URL-encoded.
    /// </summary>
    public string PeriodKey { get; set; }

    /// <summary>The VAT registration number.</summary>
    [JsonProperty("vrn")]
    public string Vrn { get; set; }

    /// <summary>The Gov-Test-Scenario, only in the sandbox environment.</summary>
    public string GovTestScenario { get; set; }

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => $"{Vrn}/returns/{HttpUtility.UrlEncode(PeriodKey)}";

    void IApiRequest.PopulateRequest(IRestRequest request) {
    }
  }
}
