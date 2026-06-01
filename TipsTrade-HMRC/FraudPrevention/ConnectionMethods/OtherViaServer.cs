using System;
using System.Collections.Generic;
using System.Net;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.FraudPrevention.ConnectionMethods {
  /// <summary>Fraud prevention headers for an application connecting through intermediary servers to HMRC via an unclassified method.</summary>
  /// <remarks>
  /// See <see href="https://developer.service.hmrc.gov.uk/guides/fraud-prevention/connection-method/other-via-server/"/> for information
  /// on which headers are required and recommended for this connection method, and how to obtain the relevant information for each header.
  /// </remarks>
  public class OtherViaServer :
    IFraudPrevention,
    IDeviceId,
    ILocalIps,
    IMacAddresses,
    IMultiFactor,
    IPublicIp,
    IPublicPort,
    ITimeZone,
    IUserAgent,
    IUserIds,
    IVendorForwarded,
    IVendorLicenceIDs,
    IVendorProductName,
    IVendorPublicIP,
    IVendorVersion {
    /// <inheritdoc/>
    public ConnectionMethod ConnectionMethod => ConnectionMethod.OTHER_VIA_SERVER;

    /// <inheritdoc/>
    public string? DeviceId { get; set; }

    /// <inheritdoc/>
    public IEnumerable<IPAddress>? LocalIPs { get; set; }

    /// <inheritdoc/>
    public DateTime LocalIPsTimestamp { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public IEnumerable<string>? MacAddresses { get; set; }

    /// <inheritdoc/>
    public IEnumerable<MultiFactor>? MultiFactor { get; set; }

    /// <inheritdoc/>
    public IPAddress? PublicIp { get; set; }

    /// <inheritdoc/>
    public DateTime PublicIpTimestamp { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public int? PublicPort { get; set; }

    /// <inheritdoc/>
    public TimeZoneInfo? TimeZone { get; set; }

    /// <inheritdoc/>
    public UserAgent? UserAgent { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, string>? UserIds { get; set; }

    /// <inheritdoc/>
    public IEnumerable<Forwarded>? VendorForwarded { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, string>? VendorLicenceIDs { get; set; }

    /// <inheritdoc/>
    public string? VendorProductName { get; set; }

    /// <inheritdoc/>
    public IPAddress? VendorPublicIP { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, string>? VendorVersion { get; set; }

    /// <inheritdoc/>
    public IEnumerable<FraudPreventionHeader> GetHeaders() => new[] {
      this.GetConnectionMethodHeader(),
      this.GetDeviceIdHeader(),
      this.GetLocalIpsHeader(),
      this.GetLocalIpsTimestampHeader(),
      this.GetMacAddressesHeader(),
      this.GetMultiFactorHeader(),
      this.GetPublicIpHeader(),
      this.GetPublicIpTimestampHeader(),
      this.GetPublicPortHeader(),
      this.GetTimeZoneHeader(),
      this.GetUserAgentHeader(),
      this.GetUserIdsHeader(),
      this.GetVendorForwardedHeader(),
      this.GetVendorLicenceIDsHeader(),
      this.GetVendorProductNameHeader(),
      this.GetVendorPublicIPHeader(),
      this.GetVendorVersionHeader(),
    };
  }
}
