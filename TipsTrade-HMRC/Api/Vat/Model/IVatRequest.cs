using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a model that provides a VAT registration number.</summary>
  public interface IVatRequest : IGovTestScenario {
    /// <summary>The VAT registration number.</summary>
    string Vrn { get; set; }
  }
}
