using System;
using System.Collections.Generic;
using System.Net;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.FraudPrevention.ConnectionMethods {
  /// <summary>Fraud prevention headers for a mobile application connecting directly to HMRC.</summary>
  /// <remarks>
  /// See <see href="https://developer.service.hmrc.gov.uk/guides/fraud-prevention/connection-method/mobile-app-direct/"/> for information
  /// on which headers are required and recommended for this connection method, and how to obtain the relevant information for each header.
  /// </remarks>
  public class MobileAppDirect :
    IFraudPrevention,
    IDeviceId,
    ILocalIps,
    IMultiFactor,
    IScreens,
    ITimeZone,
    IUserAgent,
    IUserIds,
    IWindowSize,
    IVendorLicenceIDs,
    IVendorProductName,
    IVendorVersion {
    /// <inheritdoc/>
    public ConnectionMethod ConnectionMethod => ConnectionMethod.MOBILE_APP_DIRECT;

    /// <inheritdoc/>
    public Guid? DeviceId { get; set; }

    /// <inheritdoc/>
    public IEnumerable<IPAddress>? LocalIPs { get; set; }

    /// <inheritdoc/>
    public DateTime LocalIPsTimestamp { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public IEnumerable<MultiFactor>? MultiFactor { get; set; }

    /// <inheritdoc/>
    public IEnumerable<Screen>? Screens { get; set; }

    /// <inheritdoc/>
    public TimeZoneInfo? TimeZone { get; set; }

    /// <inheritdoc/>
    public UserAgent? UserAgent { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, string>? UserIds { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, string>? VendorLicenceIDs { get; set; }

    /// <inheritdoc/>
    public string? VendorProductName { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, string>? VendorVersion { get; set; }

    /// <inheritdoc/>
    public Size? WindowSize { get; set; }

    /// <inheritdoc/>
    public IEnumerable<(string Name, string Value)> GetHeaders() {
      yield return this.GetConnectionMethod();
      yield return this.GetDeviceId();
      yield return this.GetLocalIps();
      yield return this.GetLocalIpsTimestamp();
      yield return this.GetMultiFactor();
      yield return this.GetScreens();
      yield return this.GetTimeZone();
      yield return this.GetUserAgent();
      yield return this.GetUserIds();
      yield return this.GetVendorLicenceIDs();
      yield return this.GetVendorProductName();
      yield return this.GetVendorVersion();
      yield return this.GetWindowSize();
    }
  }
}
