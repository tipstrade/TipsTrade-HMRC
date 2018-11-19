using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>The parameters used to create an individual test user.</summary>
  public class CreateIndividualRequest : IApiRequest, ICreateTestUserRequest {
    /// <summary>Generates an EORI number and enrols the user for Customs Services.</summary>
    [ServiceName]
    public const string CustomsServices = "customs-services";

    /// <summary>Generates a National Insurance number and a Making Tax Digital Income Tax ID and enrols the user for Making Tax Digital Income Tax.</summary>
    [ServiceName]
    public const string MtdIncomeTax = "mtd-income-tax";

    /// <summary>Generates a National Insurance number and enrols the user for National Insurance.</summary>
    [ServiceName]
    public const string NationalInsurance = "national-insurance";

    /// <summary>Generates a Self Assessment UTR and enrols the user for Self Assessment.</summary>
    [ServiceName]
    public const string SelfAssessment = "self-assessment";

    /// <summary>The list of services that the user should be enrolled for.</summary>
    [JsonProperty("serviceNames")]
    public List<string> ServiceNames { get; set; } = new List<string>();

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.Application;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.POST;

    string IApiRequest.Location => "individuals";

    void IApiRequest.PopulateRequest(IRestRequest request) {
      request.AddJsonBodyNewtonsoft(this);
    }
  }
}
