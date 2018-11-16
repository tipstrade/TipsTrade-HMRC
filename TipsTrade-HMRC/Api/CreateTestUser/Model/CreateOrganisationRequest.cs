using Newtonsoft.Json;
using System.Collections.Generic;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>The parameters used to create an organisation test user.</summary>
  public class CreateOrganisationRequest : ICreateTestUserRequest {
    /// <summary>Generates a Corporation Tax UTR and enrols the user for Corporation Tax.</summary>
    [ServiceName]
    public const string CorporationTax = "corporation-tax";

    /// <summary>Generates an EORI number and enrols the user for Customs Services.</summary>
    [ServiceName]
    public const string CustomsServices = "customs-services";

    /// <summary>Generates a LISA Manager Reference Number and enrols the user for Lifetime ISA.</summary>
    [ServiceName]
    public const string Lisa = "lisa";

    /// <summary>Generates a National Insurance number and a Making Tax Digital Income Tax ID and enrols the user for Making Tax Digital Income Tax.</summary>
    [ServiceName]
    public const string MtdIncomeTax = "mtd-income-tax";

    /// <summary>Generates a VAT Registration Number and enrols the user for Making Tax Digital VAT.</summary>
    [ServiceName]
    public const string MtdVat = "mtd-vat";

    /// <summary>Generates a National Insurance number and enrols the user for National Insurance.</summary>
    [ServiceName]
    public const string NationalInsurance = "national-insurance";

    /// <summary>Generates an Employer Reference and enrols the user for PAYE for Employers.</summary>
    [ServiceName]
    public const string PayeForEmployees = "paye-for-employers";

    /// <summary>Generates a Pension Scheme Administrator Identifier and enrols the user for Relief at Source.</summary>
    [ServiceName]
    public const string ReliefAtSource = "relief-at-source";

    /// <summary>Generates a Secure Electronic Transfer Reference Number and enrols the user for HMRC Secure Electronic Transfer.</summary>
    [ServiceName]
    public const string SecureElectronicTransfer = "secure-electronic-transfer";

    /// <summary>Generates a Self Assessment UTR and enrols the user for Self Assessment.</summary>
    [ServiceName]
    public const string SelfAssessment = "self-assessment";

    /// <summary>Generates a VAT Registration Number and enrols the user for Submit VAT Returns.</summary>
    [ServiceName]
    public const string SubmitVatReturns = "submit-vat-returns";

    /// <summary>The list of services that the user should be enrolled for.</summary>
    [JsonProperty("serviceNames")]
    public List<string> ServiceNames { get; set; } = new List<string>();
  }
}
