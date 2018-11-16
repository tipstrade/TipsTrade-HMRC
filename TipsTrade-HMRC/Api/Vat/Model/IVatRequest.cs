namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a model that provides a VAT registration number.</summary>
  public interface IVatRequest {
    /// <summary>The VAT registration number.</summary>
    string Vrn { get; set; }
  }
}
