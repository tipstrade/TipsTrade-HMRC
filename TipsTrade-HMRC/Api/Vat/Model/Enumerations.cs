using System.ComponentModel;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The valid status values for the VAT obligation.</summary>
  public enum ObligationStatus {
    /// <summary>Open</summary>
    [Description("O")]
    Open,
    /// <summary>Fulfilled</summary>
    [Description("F")]
    Fulfilled
  }
}
