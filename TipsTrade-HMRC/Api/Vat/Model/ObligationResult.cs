using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TipsTrade.HMRC.Json.Converters;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing VAT obligations.</summary>
  public class ObligationResult {
    /// <summary>The due date for this obligation period.</summary>
    public DateTime Due { get; set; }

    /// <summary>The end date of this obligation period.</summary>
    public DateTime End { get; set; }

    /// <summary>The ID code for the period that this obligation belongs to. The format is a string of four alphanumeric characters. Occasionally the format includes the # symbol.</summary>
    public string PeriodKey { get; set; }

    /// <summary>The obligation received date.</summary>
    public DateTime? Received { get; set; }

    /// <summary>The start date of this obligation period.</summary>
    public DateTime Start { get; set; }

    /// <summary>Which obligation statuses to return.</summary>
    [JsonConverter(typeof(CharEnumConverter))]
    public ObligationStatus Status { get; set; }
  }

  internal class ObligationsResponse {
    [JsonProperty("obligations")]
    public IEnumerable<ObligationResult> Obligations { get; set; }
  }
}
