namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT obligations.</summary>
  public class ObligationsRequest : DateRangeRequest {
    /// <summary>Which obligation statuses to return (O=Open, F=Fulfilled).</summary>
    public ObligationStatus? Status { get; set; }
  }
}
