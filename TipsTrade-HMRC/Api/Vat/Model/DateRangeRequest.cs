using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT details with a date range.</summary>
  public abstract class DateRangeRequest : IDateRange, IVatRequest {
    /// <summary>Date from which to return values.</summary>
    [JsonProperty("from"), JsonPropertyName("from")]
    public DateTime DateFrom { get; set; }

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    /// <summary>Date to which to return values.</summary>
    [JsonProperty("to"), JsonPropertyName("to")]
    public DateTime DateTo { get; set; }

    /// <summary>The VAT registration number.</summary>
    [JsonProperty("vrn"), JsonPropertyName("vrn")]
    public string Vrn { get; set; }
  }
}
