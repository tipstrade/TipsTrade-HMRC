using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-Forwarded header.</summary>
  public interface IVendorForwarded {
    /// <summary>Gets or sets a list that details hops over the internet between services that terminate TLS.</summary>
    IEnumerable<Forwarded>? VendorForwarded { get; set; }
  }

  internal static class VendorForwardedExtensions {
    internal static FraudPreventionHeader GetVendorForwardedHeader(this IVendorForwarded source) =>
      new FraudPreventionHeader("Gov-Vendor-Forwarded", false, source.VendorForwarded);
  }
}
