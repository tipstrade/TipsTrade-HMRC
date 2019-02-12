namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>The list of valid values for the length of a VAT period.</summary>
  public enum VatPeriod : byte {
    /// <summary>VAT returns are submitted monthly.</summary>
    Month = 1,
    /// <summary>VAT returns are submitted quarterly.</summary>
    Quarter = 3,
    /// <summary>VAT returns are submitted annually.</summary>
    Annual = 12
  }
}
