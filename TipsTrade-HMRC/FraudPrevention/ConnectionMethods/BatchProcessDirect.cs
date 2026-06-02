using System;
using System.Collections.Generic;
using System.Net;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.FraudPrevention.ConnectionMethods {
  /// <summary>Fraud prevention headers for a batch process connecting directly to HMRC.</summary>
  /// <remarks>
  /// See <see href="https://developer.service.hmrc.gov.uk/guides/fraud-prevention/connection-method/batch-process-direct/"/> for information
  /// on which headers are required and recommended for this connection method, and how to obtain the relevant information for each header.
  /// </remarks>
  public class BatchProcessDirect :
    IFraudPrevention,
    IDeviceId,
    ILocalIps,
    IMacAddresses,
    ITimeZone,
    IUserAgent,
    IUserIds,
    IVendorLicenceIDs,
    IVendorProductName,
    IVendorVersion {
    /// <inheritdoc/>
    public ConnectionMethod ConnectionMethod => ConnectionMethod.BATCH_PROCESS_DIRECT;

    /// <inheritdoc/>
    public Guid? DeviceId { get; set; }

    /// <inheritdoc/>
    public IEnumerable<IPAddress>? LocalIPs { get; set; }

    /// <inheritdoc/>
    public DateTime LocalIPsTimestamp { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public IEnumerable<string>? MacAddresses { get; set; }

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
    public IEnumerable<(string Name, string Value)> GetHeaders()  {
      yield return this.GetConnectionMethod();
      yield return this.GetDeviceId();
      yield return this.GetLocalIps();
      yield return this.GetLocalIpsTimestamp();
      yield return this.GetMacAddresses();
      yield return this.GetTimeZone();
      yield return this.GetUserAgent();
      yield return this.GetUserIds();
      yield return this.GetVendorLicenceIDs();
      yield return this.GetVendorProductName();
      yield return this.GetVendorVersion();
    }
  }
}
