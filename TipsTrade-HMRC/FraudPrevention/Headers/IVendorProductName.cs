using System;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-Product-Name header.</summary>
  public interface IVendorProductName {
    /// <summary>Gets or sets the name of the product marketed to end users.</summary>
    string? VendorProductName { get; set; }
  }

  internal static class VendorProductNameExtensions {
    internal static (string Name, string Value) GetVendorProductName(this IVendorProductName source) {
      var value = source.VendorProductName == null ? "" : Uri.UnescapeDataString(source.VendorProductName);

      return ("Gov-Vendor-Product-Name", value);
    }
  }
}
