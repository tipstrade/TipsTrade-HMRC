using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Json.Converters;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a result containing a VAT obligation.</summary>
  public class ObligationsResult :IComparable<ObligationsResult> {
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

    /// <summary>Compares the current object to the specified <see cref="ObligationsResult"/>, using the <see cref="Start"/> property.</summary>
    public int CompareTo(ObligationsResult other) {
      return other == null ? 1 : Start.CompareTo(other.Start);
    }
  }
}
