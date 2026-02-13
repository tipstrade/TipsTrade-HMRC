using System;
using TipsTrade.HMRC.Api.Model.Converters;
using Nsft = Newtonsoft.Json;
using Stj = System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>Represents self-employment period dates.</summary>
  public class PeriodDates {
    /// <summary>The first day that the income, expenses and deduction period summary covers.</summary>
    [Nsft.JsonProperty("periodStartDate"), Stj.JsonPropertyName("periodStartDate")]
    [Nsft.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [Stj.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime PeriodStartDate { get; set; }

    /// <summary>The last day that the income, expenses and deduction period summary covers.</summary>
    [Nsft.JsonProperty("periodEndDate"), Stj.JsonPropertyName("periodEndDate")]
    [Nsft.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [Stj.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime PeriodEndDate { get; set; }

    /// <inheritdoc/>
    public override string ToString() {
      return $"{PeriodStartDate:yyyy-MM-dd} to {PeriodEndDate:yyyy-MM-dd}";
    }
  }
}
