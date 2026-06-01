using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-License-IDs header.</summary>
  public interface IVendorLicenceIDs {
    /// <summary>Gets or sets the hashed licence keys relating to the software vendor initiating the API request.</summary>
    Dictionary<string, string>? VendorLicenceIDs { get; set; }
  }

  internal static class VendorLicenceIDsExtensions {
    internal static FraudPreventionHeader GetVendorLicenceIDsHeader(this IVendorLicenceIDs source) =>
      new FraudPreventionHeader("Gov-Vendor-License-IDs", true, source.VendorLicenceIDs);
  }
}
