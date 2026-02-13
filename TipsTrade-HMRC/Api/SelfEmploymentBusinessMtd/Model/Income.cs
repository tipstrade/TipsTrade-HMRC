using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  public class Income {
    /// <summary>The takings, fees, sales or money earned by the business. Income associated with the running of the business.</summary>
    /// <remarks>The value must be between 0 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("turnover"), JsonPropertyName("turnover")]
    public decimal Turnover { get; set; }

    /// <summary>Any other business income not included in turnover. Income associated with the running of the business.</summary>
    /// <remarks>The value must be between 0 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("other"), JsonPropertyName("other")]
    public decimal Other { get; set; }

    /// <summary>Other tax taken off trading income apart from CIS deductions.</summary>
    /// <remarks>The value must be between 0 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("taxTakenOffTradingIncome"), JsonPropertyName("taxTakenOffTradingIncome")]
    public decimal TaxTakenOffTradingIncome { get; set; }
  }
}
