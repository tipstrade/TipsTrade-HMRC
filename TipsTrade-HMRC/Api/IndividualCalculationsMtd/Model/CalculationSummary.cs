using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model.Converters;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>Represents a calculation summary.</summary>
  public class CalculationSummary {
    /// <summary>The unique identifier of the calculation.</summary>
    [JsonProperty("calculationId"), JsonPropertyName("calculationId")]
    public string Id { get; set; }

    /// <summary>The timestamp of when the calculation was performed.</summary>
    [JsonProperty("calculationTimestamp"), JsonPropertyName("calculationTimestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>The type of calculation performed.</summary>
    /// <remarks>Enum: "in-year" "intent-to-finalise" "final-declaration" "intent-to-amend" "confirm-amendment"</remarks>
    [JsonProperty("calculationType"), JsonPropertyName("calculationType")]
    public string CalculationType { get; set; }

    /// <summary>The optional requestor of the calculation.</summary>
    /// <remarks>Enum: "customer" "hmrc" "agent"</remarks>
    [JsonProperty("requestedBy"), JsonPropertyName("requestedBy")]
    public string RequestedBy { get; set; }

    /// <summary>The optional tax year the calculation was requested for.</summary>
    [JsonProperty("taxYear"), JsonPropertyName("taxYear")]
    public string TaxYear { get; set; }

    /// <summary>The optional total amount of Income Tax and National Insurance Contributions due</summary>
    [JsonProperty("totalIncomeTaxAndNicsDue"), JsonPropertyName("totalIncomeTaxAndNicsDue")]
    public decimal? TotalIncomeTaxAndNicsDue { get; set; }

    /// <summary>An optional boolean to indicate whether the calculation can be used to make a final declaration.</summary>
    [JsonProperty("intentToSubmitFinalDeclaration"), JsonPropertyName("intentToSubmitFinalDeclaration")]
    public bool? IntentToSubmitFinalDeclaration { get; set; }

    /// <summary>An optional boolean to indicate whether the calculation has been used to make a final declaration.</summary>
    [JsonProperty("finalDeclaration"), JsonPropertyName("finalDeclaration")]
    public bool? FinalDeclaration { get; set; }

    /// <summary>The optional timestamp of when the final calculation was performed.</summary>
    [JsonProperty("finalDeclarationTimestamp"), JsonPropertyName("finalDeclarationTimestamp")]
    public DateTime? FinalDeclarationTimestamp { get; set; }

    /// <summary>The optional trigger for the calculation.</summary>
    [JsonProperty("calculationTrigger"), JsonPropertyName("calculationTrigger")]
    public string CalculationTrigger { get; set; }

    /// <summary>The optional outcome of the calculation request.</summary>
    [JsonProperty("calculationOutcome"), JsonPropertyName("calculationOutcome")]
    public string CalculationOutcome { get; set; }

    /// <summary>This is one of, 'totalIncomeTaxAndNicsDue' or 'totalIncomeTaxAndNicsAndCgt' if present, from the calculation output.</summary>
    [JsonProperty("liabilityAmount"), JsonPropertyName("liabilityAmount")]
    public decimal? LiabilityAmount { get; set; }

    /// <summary>The optional start of the period covered by the calculation.</summary>
    [JsonProperty("fromDate"), JsonPropertyName("fromDate")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime? FromDate { get; set; }

    /// <summary>The optional end of the period covered by the calculation.</summary>
    [JsonProperty("toDate"), JsonPropertyName("toDate")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime? ToDate { get; set; }

    /// <inheritdoc/>
    public override string ToString() {
      return $"{FromDate:yyyy-MM-dd} to {ToDate:yyyy-MM-dd} - {CalculationType} {CalculationOutcome}: {LiabilityAmount:#.00}".Trim();
    }
  }
}
