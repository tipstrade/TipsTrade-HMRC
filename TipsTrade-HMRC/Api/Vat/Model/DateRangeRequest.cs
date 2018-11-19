using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT details with a date range.</summary>
  public class DateRangeRequest : IDateRange, IVatRequest {
    /// <summary>Date from which to return values.</summary>
    [JsonProperty("from")]
    public DateTime From { get; set; }

    /// <summary>Date to which to return values.</summary>
    [JsonProperty("to")]
    public DateTime To { get; set; }

    /// <summary>The VAT registration number.</summary>
    [JsonProperty("vrn")]
    public string Vrn { get; set; }
  }
}
