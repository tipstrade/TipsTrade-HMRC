using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace TipsTrade.HMRC.AntiFraud {
  /// <summary>Represents an object that contains multi-factor authentication information.</summary>
  public class UserAgent : IAntiFraudValue {
    /// <summary>Gets or sets the device manufacturer.</summary>
    public string DeviceManufacturer { get; set; }

    /// <summary>Gets or sets the device model.</summary>
    public string DeviceModel { get; set; }
    
    /// <summary>Gets or sets the operating system family.</summary>
    public string OSFamily { get; set; }

    /// <summary>Gets or sets the operating system version.</summary>
    public string OSVersion { get; set; }

    /// <summary>Retuns a string that contains the anti fraud header value.</summary>
    public string GetHeaderValue() {
      var dict = new Dictionary<string, string> {
        {"os-family", OSFamily },
        {"os-version", OSVersion },
        {"device-manufacturer", DeviceManufacturer },
        {"device-model", DeviceModel }
      };

      return dict.GetHeaderValue();
    }
  }
}
