using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model.Converters;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>Represents the top-level metadata information about the retrieved tax calculation.</summary>
  public class CalculationMetadata {
    /// <summary>
    /// The unique identifier of the calculation.
    /// </summary>
    [JsonProperty("calculationId"), JsonPropertyName("calculationId")]
    public string CalculationId { get; set; }

    /// <summary>
    /// The tax year in which the calculation was performed.
    /// </summary>
    [JsonProperty("taxYear"), JsonPropertyName("taxYear")]
    public string TaxYear { get; set; }

    /// <summary>
    /// The requester of the calculation. Currently "customer" will be returned even in the case of an agent submission.
    /// </summary>
    [JsonProperty("requestedBy"), JsonPropertyName("requestedBy")]
    public string RequestedBy { get; set; }

    /// <summary>
    /// The timestamp of when the calculation was requested.
    /// </summary>
    [JsonProperty("requestedTimestamp"), JsonPropertyName("requestedTimestamp")]
    public DateTime? RequestedTimestamp { get; set; }

    /// <summary>
    /// The reason why the calculation was triggered.
    /// </summary>
    [JsonProperty("calculationReason"), JsonPropertyName("calculationReason")]
    public string CalculationReason { get; set; }

    /// <summary>
    /// The timestamp of when the calculation was performed.
    /// </summary>
    [JsonProperty("calculationTimestamp"), JsonPropertyName("calculationTimestamp")]
    public DateTime? CalculationTimestamp { get; set; }

    /// <summary>
    /// The type of calculation performed.
    /// </summary>
    [JsonProperty("calculationType"), JsonPropertyName("calculationType")]
    public string CalculationType { get; set; }

    /// <summary>
    /// A boolean to indicate whether the calculation can be used to make a final declaration. This value must be true or false.
    /// </summary>
    [JsonProperty("intentToSubmitFinalDeclaration"), JsonPropertyName("intentToSubmitFinalDeclaration")]
    public bool IntentToSubmitFinalDeclaration { get; set; }

    /// <summary>
    /// A boolean to indicate whether the calculation has been used to make a final declaration. This value must be true or false.
    /// </summary>
    [JsonProperty("finalDeclaration"), JsonPropertyName("finalDeclaration")]
    public bool? FinalDeclaration { get; set; }

    /// <summary>
    /// The timestamp of when the calculation was performed.
    /// </summary>
    [JsonProperty("finalDeclarationTimestamp"), JsonPropertyName("finalDeclarationTimestamp")]
    public DateTime? FinalDeclarationTimestamp { get; set; }

    /// <summary>The timestamp when the finalisation was performed.</summary>
    [JsonProperty("finalisationTimestamp"), JsonPropertyName("finalisationTimestamp")]
    public DateTime? FinalisationTimestamp { get; set; }

    /// <summary>The timestamp when the confirmation was performed.</summary>
    [JsonProperty("confirmationTimestamp"), JsonPropertyName("confirmationTimestamp")]
    public DateTime? ConfirmationTimestamp { get; set; }

    /// <summary>
    /// This defines the earliest date from which the calculation was performed. This will be the accounting period start date (for self-employments) or the tax year start date, whichever occurs first for the income sources supplied.
    /// </summary>
    [JsonProperty("periodFrom"), JsonPropertyName("periodFrom")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime PeriodFrom { get; set; }

    /// <summary>
    /// This defines the latest date to which the calculation was performed and will be the point in the year to which income has been submitted.
    /// </summary>
    [JsonProperty("periodTo"), JsonPropertyName("periodTo")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime PeriodTo { get; set; }
  }
}
