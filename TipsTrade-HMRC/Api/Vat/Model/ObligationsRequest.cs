using System;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT obligations.</summary>
  public class ObligationsRequest : IVatRequest {
    /// <summary>Date from which to return obligations.</summary>
    public DateTime From { get; set; }

    /// <summary>Date to which to return obligations.</summary>
    public DateTime To { get; set; }

    /// <summary>Which obligation statuses to return (O=Open, F=Fulfilled).</summary>
    public ObligationStatus Status { get; set; }

    /// <summary>The VAT registration number.</summary>
    public string Vrn { get; set; }
  }
}
