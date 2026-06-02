using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Local-IPs and Gov-Client-Local-IPs-Timestamp headers.</summary>
  public interface ILocalIps {
    /// <summary>Gets or sets the list of all local IP addresses available to the originating device.</summary>
    IEnumerable<IPAddress>? LocalIPs { get; set; }

    /// <summary>Gets or sets the timestamp recording when <see cref="LocalIPs"/> was collected.</summary>
    DateTime LocalIPsTimestamp { get; set; }
  }

  /// <summary>
  /// Extension methods for <see cref="ILocalIps"/> to populate the local IPs and generate the corresponding headers.
  /// </summary>
  public static class LocalIpsExtensions {
    internal static (string Name, string Value) GetLocalIps(this ILocalIps source) {
      return ("Gov-Client-Local-IPs", source.LocalIPs.EncodeIpAddresses());
    }

    internal static (string Name, string Value) GetLocalIpsTimestamp(this ILocalIps source) {
      return ("Gov-Client-Local-IPs-Timestamp", source.LocalIPsTimestamp.EncodeTimestamp());
    }

    /// <summary>Populates <see cref="ILocalIps.LocalIPs"/> with all local IP addresses.</summary>
    public static void PopulateLocalIps(this ILocalIps source, Func<IPAddress, bool>? predicate = null) {
      var ips = NetworkInterface.GetAllNetworkInterfaces()
        .GetAllAddresses()
        .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || x.IsIPv6LinkLocal);
      if (predicate != null) ips = ips.Where(predicate);
      source.LocalIPs = ips.ToArray();
      source.LocalIPsTimestamp = DateTime.UtcNow;
    }
  }
}
