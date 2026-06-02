using System;
using System.Collections.Generic;
using System.Net;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.FraudPrevention.ConnectionMethods {
  /// <summary>Fraud prevention headers for a web application connecting through intermediary servers to HMRC.</summary>
  /// <remarks>
  /// See <see href="https://developer.service.hmrc.gov.uk/guides/fraud-prevention/connection-method/web-app-via-server/"/> for information
  /// on which headers are required and recommended for this connection method, and how to obtain the relevant information for each header.
  /// </remarks>
  public class WebAppViaServer :
    IFraudPrevention,
    IBrowserJavaScriptUserAgent,
    IDeviceId,
    IMultiFactor,
    IPublicIp,
    IPublicPort,
    IScreens,
    ITimeZone,
    IUserIds,
    IVendorForwarded,
    IVendorLicenceIDs,
    IVendorProductName,
    IVendorPublicIP,
    IVendorVersion,
    IWindowSize {
    /// <inheritdoc/>
    public ConnectionMethod ConnectionMethod => ConnectionMethod.WEB_APP_VIA_SERVER;

    /// <inheritdoc/>
    public string? BrowserJavaScriptUserAgent { get; set; }

    /// <inheritdoc/>
    public Guid? DeviceId { get; set; }

    /// <inheritdoc/>
    public IEnumerable<MultiFactor>? MultiFactor { get; set; }

    /// <inheritdoc/>
    public IPAddress? PublicIp { get; set; }

    /// <inheritdoc/>
    public DateTime PublicIpTimestamp { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public int? PublicPort { get; set; }

    /// <inheritdoc/>
    public IEnumerable<Screen>? Screens { get; set; }

    /// <inheritdoc/>
    public TimeZoneInfo? TimeZone { get; set; }

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
    public Size? WindowSize { get; set; }

    /// <inheritdoc/>
    public IEnumerable<(string Name, string Value)> GetHeaders()  {
      yield return this.GetConnectionMethod();
      yield return this.GetBrowserJavaScriptUserAgent();
      yield return this.GetDeviceId();
      yield return this.GetMultiFactor();
      yield return this.GetPublicIp();
      yield return this.GetPublicIpTimestamp();
      yield return this.GetPublicPort();
      yield return this.GetScreens();
      yield return this.GetTimeZone();
      yield return this.GetUserIds();
      yield return this.GetVendorForwarded();
      yield return this.GetVendorLicenceIDs();
      yield return this.GetVendorProductName();
      yield return this.GetVendorPublicIP();
      yield return this.GetVendorVersion();
      yield return this.GetWindowSize();
    }
  }
}
