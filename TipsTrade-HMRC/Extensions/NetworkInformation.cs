using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace System.Net.NetworkInformation {
internal static   class NetworkInformation {
    public static string FormatMAC(this PhysicalAddress mac) {
      return string.Join(":", mac.GetAddressBytes().Select(b => $"{b:x2}"));
    }

    /// <summary>Returns a list of all the unicast InterNetwork ip addresses for the current collection of interfaces.</summary>
    public static IEnumerable<IPAddress> GetAllAddresses(this IEnumerable<NetworkInterface> interfaces) {
      var foo = interfaces.Select(i => i.GetIPProperties());

      return interfaces
        .Where(i => i.NetworkInterfaceType != NetworkInterfaceType.Loopback)
        .SelectMany(i => {
          return i.GetIPProperties().UnicastAddresses
          .Where(a => {
            return !a.Address.IsIPv6LinkLocal
            && (a.Address.AddressFamily == AddressFamily.InterNetwork || a.Address.AddressFamily == AddressFamily.InterNetworkV6);
          })
          .Select(a => a.Address);
        });
    }

    /// <summary>Returns a list of all the MAC addresses for the current collection of interfaces.</summary>
    public static IEnumerable<PhysicalAddress> GetAllMACAddresses(this IEnumerable<NetworkInterface> interfaces) {
      return interfaces
        .Where(i => i.NetworkInterfaceType != NetworkInterfaceType.Loopback)
        .Select(i => i.GetPhysicalAddress());
    }
  }
}
