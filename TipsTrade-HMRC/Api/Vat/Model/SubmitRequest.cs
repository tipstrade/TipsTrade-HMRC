using System;
using Newtonsoft.Json;
using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to submit a VAT return.</summary>
  public class SubmitRequest : IApiRequest, IVatRequest {
    /// <summary>The remote endpoint has indicated that VAT has already been submitted for that period.</summary>
    [GovTestScenario]
    public const string ScenarioDuplicateSubmission = "DUPLICATE_SUBMISSION";

    /// <summary>The remote endpoint has indicated that the Agent Reference Number is invalid.</summary>
    [GovTestScenario]
    [Obsolete]
    public const string ScenarioInvalidArn = "INVALID_ARN";

    /// <summary>Submission has not passed validation. Invalid parameter PERIODKEY.</summary>
    [GovTestScenario]
    public const string ScenarioInvalidPeriodKey = "INVALID_PERIODKEY";

    /// <summary>Submission has not passed validation. Invalid parameter Payload.</summary>
    [GovTestScenario]
    public const string ScenarioInvalidPayload = "INVALID_PAYLOAD";

    /// <summary>Submission has not passed validation. Invalid parameter VRN.</summary>
    [GovTestScenario]
    public const string ScenarioInvalidVrn = "INVALID_VRN";

    /// <summary>The remote endpoint has indicated that the submission is for a tax period that has not ended.</summary>
    [GovTestScenario]
    public const string ScenarioTaxPeriodNotEnded = "TAX_PERIOD_NOT_ENDED";

    /// <summary>The VAT registration number.</summary>
    [JsonProperty("vrn")]
    public string Vrn { get; set; }

    /// <summary>The Gov-Test-Scenario, only in the sandbox environment.</summary>
    public string GovTestScenario { get; set; }

    /// <summary>The VAT return to be submitted.</summary>
    public VatReturn Return { get; set; }

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.POST;

    string IApiRequest.Location => $"{Vrn}/returns";

    void IApiRequest.PopulateRequest(IRestRequest request) {
      if (Return.Finalised == null) {
        throw new InvalidOperationException($"{nameof(Return.Finalised)} cannot be null.");
      }

      request.AddJsonBodyNewtonsoft(Return);
    }
  }
}
