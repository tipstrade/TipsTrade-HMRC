using System.Collections;
using System.Collections.Generic;
using System.Web;

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

    /// <summary>Returns a string that contains the fraud prevention header value.</summary>
    public string GetHeaderValue() {
      var dict = new Dictionary<string, string> {
        {"os-family", HttpUtility.UrlEncode(OSFamily) },
        {"os-version", HttpUtility.UrlEncode(OSVersion) },
        {"device-manufacturer", HttpUtility.UrlEncode(DeviceManufacturer) },
        {"device-model", HttpUtility.UrlEncode(DeviceModel) }
      };

      return dict.GetHeaderValue();
    }
  }
}
