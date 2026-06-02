using System.Collections.Generic;
using System.Linq;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-Forwarded header.</summary>
  public interface IVendorForwarded {
    /// <summary>Gets or sets a list that details hops over the internet between services that terminate TLS.</summary>
    IEnumerable<Forwarded>? VendorForwarded { get; set; }
  }

  internal static class VendorForwardedExtensions {
    internal static (string Name, string Value) GetVendorForwarded(this IVendorForwarded source) {
      var value = source.VendorForwarded == null ? "" : string.Join(",", source.VendorForwarded.Select(x => x.GetHeaderValue()));

      return ("Gov-Vendor-Forwarded", value);
    }
  }
}
