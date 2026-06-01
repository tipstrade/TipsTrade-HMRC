using System;
using System.Net;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Public-IP header.</summary>
  public interface IPublicIp {
    /// <summary>Gets or sets the public IP address from which the originating device makes the request.</summary>
    IPAddress? PublicIp { get; set; }

    /// <summary>Gets or sets the timestamp recording when <see cref="PublicIp"/> was collected.</summary>
    DateTime PublicIpTimestamp { get; set; }
  }

  internal static class PublicIpExtensions {
    internal static FraudPreventionHeader GetPublicIpHeader(this IPublicIp source) =>
      new FraudPreventionHeader("Gov-Client-Public-IP", false, source.PublicIp);

    internal static FraudPreventionHeader GetPublicIpTimestampHeader(this IPublicIp source) =>
      new FraudPreventionHeader("Gov-Client-Public-IP-Timestamp", true, source.PublicIpTimestamp);
  }
}
