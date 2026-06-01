using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-Version header.</summary>
  public interface IVendorVersion {
    /// <summary>Gets or sets the list of software versions involved in handling the request.</summary>
    Dictionary<string, string>? VendorVersion { get; set; }
  }

  internal static class VendorVersionExtensions {
    internal static FraudPreventionHeader GetVendorVersionHeader(this IVendorVersion source) =>
      new FraudPreventionHeader("Gov-Vendor-Version", false, source.VendorVersion);
  }
}
