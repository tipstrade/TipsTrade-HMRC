using Newtonsoft.Json;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents a response containing a created organisation.</summary>
  [RequestType(typeof(CreateOrganisationRequest))]
  public class OrganisationResult : UserResultBase {
    /// <summary>Organisation deatils.</summary>
    [JsonProperty("organisationDetails")]
    public Organisation OrganisationDetails { get; set; }

    /// <summary>Self Assessment UTR.</summary>
    [JsonProperty("saUtr")]
    public string SelfAssessmentUtr { get; set; }

    /// <summary>National Insurance number</summary>
    [JsonProperty("nino")]
    public string NiNumber { get; set; }

    /// <summary>Employer Reference.</summary>
    [JsonProperty("empRef")]
    public string EmployerReference { get; set; }

    /// <summary>Corporation Tax UTR.</summary>
    [JsonProperty("ctUtr")]
    public string CorporationTaxUtr { get; set; }

    /// <summary>VAT Reference Number</summary>
    [JsonProperty("vrn")]
    public string Vrn { get; set; }

    /// <summary>Making Tax Digital Income Tax ID</summary>
    [JsonProperty("mtdItId")]
    public string MtdIncomeTaxId { get; set; }

    /// <summary>LISA Manager Reference Number, in either 4-digit format (Znnnn) or 6-digit format (Znnnnnn).</summary>
    [JsonProperty("lisaManagerReferenceNumber")]
    public string LisaManagerReferenceNumber { get; set; }

    /// <summary>Secure Electronic Transfer reference number.</summary>
    [JsonProperty("secureElectronicTransferReferenceNumber")]
    public string SecureElectronicTransferReferenceNumber { get; set; }

    /// <summary>Pension Scheme Administrator Identifier.</summary>
    [JsonProperty("pensionSchemeAdministratorIdentifier")]
    public string PensionSchemeAdministratorIdentifier { get; set; }

    /// <summary>Economic Operator Registration and Identification (EORI) number.</summary>
    [JsonProperty("eoriNumber")]
    public string EoriNumber { get; set; }

    /// <summary>Represents an organisation.</summary>
    public class Organisation {
      /// <summary>Organisation name</summary>
      [JsonProperty("name")]
      public string Name { get; set; }

      /// <summary>Organisation address.</summary>
      [JsonProperty("address")]
      public Address Address { get; set; }

      /// <summary>Returns a string that represents the current object.</summary>
      public override string ToString() {
        return $"{Name}";
      }
    }
  }
}
