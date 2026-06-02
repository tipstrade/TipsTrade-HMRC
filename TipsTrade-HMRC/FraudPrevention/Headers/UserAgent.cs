using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Represents an object that contains user agent information.</summary>
  public class UserAgent : IFraudPreventionValue {
    /// <summary>Gets or sets the device manufacturer.</summary>
    public string DeviceManufacturer { get; set; } = "";

    /// <summary>Gets or sets the device model.</summary>
    public string DeviceModel { get; set; } = "";

    /// <summary>Gets or sets the operating system family.</summary>
    public string OSFamily { get; set; } = "";

    /// <summary>Gets or sets the operating system version.</summary>
    public string OSVersion { get; set; } = "";

    /// <inheritdoc/>
    public string GetHeaderValue() {
      var dict = new Dictionary<string, string> {
        {"os-family", OSFamily },
        {"os-version", OSVersion },
        {"device-manufacturer", DeviceManufacturer },
        {"device-model", DeviceModel }
      };

      // As per documentation, both the key and value of the dictionary should be URL encoded.
      return dict.EncodeKeyValues(true);
    }
  }
}
