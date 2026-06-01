using System.Net;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Vendor-Public-IP header.</summary>
  public interface IVendorPublicIP {
    /// <summary>Gets or sets the public IP address from which the vendor's servers make the request.</summary>
    IPAddress? VendorPublicIP { get; set; }
  }

  internal static class VendorPublicIPExtensions {
    internal static FraudPreventionHeader GetVendorPublicIPHeader(this IVendorPublicIP source) =>
      new FraudPreventionHeader("Gov-Vendor-Public-IP", false, source.VendorPublicIP);
  }
}
