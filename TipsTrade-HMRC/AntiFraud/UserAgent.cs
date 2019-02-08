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
      return string.Format("{0}/{1} ({2}/{3})",
        HttpUtility.UrlEncode(OSFamily ?? ""),
        HttpUtility.UrlEncode(OSVersion ?? ""),
        HttpUtility.UrlEncode(DeviceManufacturer ?? ""),
        HttpUtility.UrlEncode(DeviceModel ?? "")
        );
    }
  }
}
