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
    internal static (string Name, string Value) GetPublicIp(this IPublicIp source) {
      return ("Gov-Client-Public-IP", source.PublicIp.EncodeIpAddress());
    }

    internal static (string Name, string Value) GetPublicIpTimestamp(this IPublicIp source) {
      return ("Gov-Client-Public-IP-Timestamp", source.PublicIpTimestamp.EncodeTimestamp());
    }
  }
}
