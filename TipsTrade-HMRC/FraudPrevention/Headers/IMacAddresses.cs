using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-MAC-Addresses header.</summary>
  public interface IMacAddresses {
    /// <summary>Gets or sets the list of MAC addresses available on the originating device.</summary>
    IEnumerable<string>? MacAddresses { get; set; }
  }

  /// <summary>
  /// Provides extension methods for the <see cref="IMacAddresses"/> interface.
  /// </summary>
  public static class MacAddressesExtensions {
    internal static (string Name, string Value) GetMacAddresses(this IMacAddresses source) {
      return ("Gov-Client-MAC-Addresses", source.MacAddresses.EncodeMacAddresses());
    }

    /// <summary>Populates <see cref="IMacAddresses.MacAddresses"/> with all local MAC addresses.</summary>
    public static void PopulateMacAddresses(this IMacAddresses macAddresses) {
      macAddresses.MacAddresses = NetworkInterface.GetAllNetworkInterfaces()
        .GetAllMACAddresses()
        .Select(m => m.FormatMAC())
        .Distinct()
        .ToArray();
    }
  }
}
