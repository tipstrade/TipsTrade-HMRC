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
    internal static FraudPreventionHeader GetMacAddressesHeader(this IMacAddresses source) =>
      new FraudPreventionHeader("Gov-Client-MAC-Addresses", true, source.MacAddresses);

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
