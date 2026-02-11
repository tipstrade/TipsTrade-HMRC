using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents a response containing a created organisation.</summary>
  [RequestType(typeof(CreateOrganisationRequest))]
  public class OrganisationResult : UserResultBase {
    /// <summary>Organisation deatils.</summary>
    [JsonProperty("organisationDetails"), JsonPropertyName("organisationDetails")]
    public Organisation OrganisationDetails { get; set; }

    /// <summary></summary>
    [JsonProperty("individualDetails"), JsonPropertyName("individualDetails")]
    public Individual IndividualDetails { get; set; }

    /// <summary>Self Assessment UTR.</summary>
    [JsonProperty("saUtr"), JsonPropertyName("saUtr")]
    public string SelfAssessmentUtr { get; set; }

    /// <summary>National Insurance number</summary>
    [JsonProperty("nino"), JsonPropertyName("nino")]
    public string NiNumber { get; set; }

    /// <summary>Employer Reference.</summary>
    [JsonProperty("empRef"), JsonPropertyName("empRef")]
    public string EmployerReference { get; set; }

    /// <summary>Company Reference Number.</summary>
    [JsonProperty("crn"), JsonPropertyName("crn")]
    public string Crn { get; set; }

    /// <summary>Corporation Tax UTR.</summary>
    [JsonProperty("ctUtr"), JsonPropertyName("ctUtr")]
    public string CorporationTaxUtr { get; set; }

    /// <summary>Date of registration for VAT.</summary>
    [JsonProperty("vatRegistrationDate"), JsonPropertyName("vatRegistrationDate")]
    public DateTime VatRegistrationDate { get; set; }

    /// <summary>VAT Reference Number</summary>
    [JsonProperty("vrn"), JsonPropertyName("vrn")]
    public string Vrn { get; set; }

    /// <summary>Making Tax Digital Income Tax ID</summary>
    [JsonProperty("mtdItId"), JsonPropertyName("mtdItId")]
    public string MtdIncomeTaxId { get; set; }

    /// <summary>LISA Manager Reference Number, in either 4-digit format (Znnnn) or 6-digit format (Znnnnnn).</summary>
    [JsonProperty("lisaManagerReferenceNumber"), JsonPropertyName("lisaManagerReferenceNumber")]
    public string LisaManagerReferenceNumber { get; set; }

    /// <summary>Secure Electronic Transfer reference number.</summary>
    [JsonProperty("secureElectronicTransferReferenceNumber"), JsonPropertyName("secureElectronicTransferReferenceNumber")]
    public string SecureElectronicTransferReferenceNumber { get; set; }

    /// <summary>Pension Scheme Administrator Identifier.</summary>
    [JsonProperty("pensionSchemeAdministratorIdentifier"), JsonPropertyName("pensionSchemeAdministratorIdentifier")]
    public string PensionSchemeAdministratorIdentifier { get; set; }

    /// <summary>Pillar 2 ID.</summary>
    [JsonProperty("pillar2Id"), JsonPropertyName("pillar2Id")]
    public string Pillar2Id { get; set; }

    /// <summary>Economic Operator Registration and Identification (EORI) number.</summary>
    [JsonProperty("eoriNumber"), JsonPropertyName("eoriNumber")]
    public string EoriNumber { get; set; }

    /// <summary>Excise Number.</summary>
    [JsonProperty("exciseNumber"), JsonPropertyName("exciseNumber")]
    public string ExciseNumber { get; set; }

    /// <summary>Type of Self Assessment taxpayer One of 'Individual' or 'Partnership'.</summary>
    [JsonProperty("taxPayerType"), JsonPropertyName("taxPayerType")]
    public string TaxPayerType { get; set; }

    /// <summary>ISA Manager Z Reference, in 4-digit format (Znnnn).</summary>
    [JsonProperty("zReference"), JsonPropertyName("zReference")]
    public string ZReference { get; set; }

    /// <summary>Represents an individual.</summary>
    public class Individual {
      /// <summary>Individual's first name.</summary>
      [JsonProperty("dateOfBirth"), JsonPropertyName("dateOfBirth")]
      public DateTime DateOfBirth { get; set; }

      /// <summary>Individual's last name.</summary>
      [JsonProperty("firstName"), JsonPropertyName("firstName")]
      public string FirstName { get; set; }

      /// <summary>Individual's date of birth.</summary>
      [JsonProperty("lastName"), JsonPropertyName("lastName")]
      public string LastName { get; set; }

      /// <summary>Organisation address.</summary>
      [JsonProperty("address"), JsonPropertyName("address")]
      public Address Address { get; set; }

      /// <summary>Returns a string that represents the current object.</summary>
      public override string ToString() {
        return $"{FirstName} {LastName}";
      }
    }

    /// <summary>Represents an organisation.</summary>
    public class Organisation {
      /// <summary>Organisation name</summary>
      [JsonProperty("name"), JsonPropertyName("name")]
      public string Name { get; set; }

      /// <summary>Organisation address.</summary>
      [JsonProperty("address"), JsonPropertyName("address")]
      public Address Address { get; set; }

      /// <summary>Returns a string that represents the current object.</summary>
      public override string ToString() {
        return $"{Name}";
      }
    }
  }
}
