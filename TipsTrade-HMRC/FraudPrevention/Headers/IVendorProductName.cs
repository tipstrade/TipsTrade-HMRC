namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-Product-Name header.</summary>
  public interface IVendorProductName {
    /// <summary>Gets or sets the name of the product marketed to end users.</summary>
    string? VendorProductName { get; set; }
  }

  internal static class VendorProductNameExtensions {
    internal static FraudPreventionHeader GetVendorProductNameHeader(this IVendorProductName source) =>
      new FraudPreventionHeader("Gov-Vendor-Product-Name", true, source.VendorProductName);
  }
}
