using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-Version header.</summary>
  public interface IVendorVersion {
    /// <summary>Gets or sets the list of software versions involved in handling the request.</summary>
    Dictionary<string, string>? VendorVersion { get; set; }
  }

  internal static class VendorVersionExtensions {
    internal static (string Name, string Value) GetVendorVersion(this IVendorVersion source) {
      // As per documentation, both the key and value of the dictionary should be URL encoded.
      var value = source.VendorVersion == null ? "" : source.VendorVersion.EncodeKeyValues(true);

      return ("Gov-Vendor-Version", value);
    }
  }
}
