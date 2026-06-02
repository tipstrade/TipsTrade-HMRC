using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-License-IDs header.</summary>
  public interface IVendorLicenceIDs {
    /// <summary>Gets or sets the hashed licence keys relating to the software vendor initiating the API request.</summary>
    Dictionary<string, string>? VendorLicenceIDs { get; set; }
  }

  internal static class VendorLicenceIDsExtensions {
    internal static (string Name, string Value) GetVendorLicenceIDs(this IVendorLicenceIDs source) {
      // As per documentation, both the key and value of the dictionary should be URL encoded.
      var value = source.VendorLicenceIDs == null ? "" : source.VendorLicenceIDs.EncodeDictionary(true);

      return ("Gov-Vendor-License-IDs", value);
    }
  }
}
