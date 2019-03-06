using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Web;
using TipsTrade.HMRC.AntiFraud.Attributes;

namespace TipsTrade.HMRC.AntiFraud {
  /// <summary>Represents the fraud prevention information that may be required by APIs.</summary>
  public class AntiFraud {
    #region Fields
    #endregion

    #region Properties
    /// <summary>Gets or sets a flag indicating whether the Do Not Track option is enabled on the browser.</summary>
    [AntiFraudHeader("Gov-Client-Browser-Do-Not-Track", true)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    public bool? BrowserDoNotTrack { get; set; }

    /// <summary>Gets or sets the JavaScript-reported user agent string from the originating device.</summary>
    [AntiFraudHeader("Gov-Client-Browser-JS-User-Agent")]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    public string BrowserJavaScriptUserAgent { get; set; }

    /// <summary>Gets or sets the list of browser plugins on the originating device.</summary>
    [AntiFraudHeader("Gov-Client-Browser-Plugins", true)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    public IEnumerable<string> BrowserPlugins { get; set; }

    /// <summary>Gets or sets the connection method being used.</summary>
    [AntiFraudHeader("Gov-Client-Connection-Method")]
    public ConnectionMethod ConnectionMethod { get; set; }

    /// <summary>Gets or sets the identifier unique to an originating device.</summary>
    [AntiFraudHeader("Gov-Client-Device-ID", true)]
    [ConnectionMethod(true)]
    public string DeviceID { get; set; }

    /// <summary>Gets or sets the list of all the local IP addresses (IPv4 and IPv6) available to the originating device.</summary>
    [AntiFraudHeader("Gov-Client-Local-IPs", true)]
    [ConnectionMethod(true)]
    public IEnumerable<IPAddress> LocalIPs { get; set; }

    /// <summary>Gets or sets a list of the MAC addreses available on the originating device.</summary>
    [AntiFraudHeader("Gov-Client-MAC-Addresses", true)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.BATCH_PROCESS_DIRECT)]
    [ConnectionMethod(ConnectionMethod.OTHER_DIRECT)]
    [ConnectionMethod(ConnectionMethod.OTHER_VIA_SERVER)]
    public IEnumerable<string> MACAddresses { get; set; }

    /// <summary>
    /// Gets or sets a list of key-value data structures containing details of the multi-factor
    /// authentication (MFA) statuses related to the API call.
    /// </summary>
    [AntiFraudHeader("Gov-Client-Multi-Factor", true)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.OTHER_DIRECT)]
    [ConnectionMethod(ConnectionMethod.OTHER_VIA_SERVER)]
    public IEnumerable<MultiFactor> MultiFactor { get; set; }

    /// <summary>Gets or sets the public IP address (IPv4 or IPv6) from which the originating device makes the request.</summary>
    [AntiFraudHeader("Gov-Client-Public-IP")]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.OTHER_VIA_SERVER)]
    public IPAddress PublicAddress { get; set; }

    /// <summary>Gets or sets the public TCP port that the originating device uses when initiating the request.</summary>
    [AntiFraudHeader("Gov-Client-Public-Port")]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.OTHER_VIA_SERVER)]
    public int? PublicPort { get; set; }

    /// <summary>Gets or sets the list of information related to the originating device’s screens.</summary>
    [AntiFraudHeader("Gov-Client-Screens")]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    public IEnumerable<Screen> Screens { get; set; }

    /// <summary>Gets or sets the local timezone of the originating device.</summary>
    [AntiFraudHeader("Gov-Client-Timezone")]
    [ConnectionMethod(true)]
    public TimeZoneInfo TimeZone { get; set; }

    /// <summary>
    /// Gets or sets the operating system family, version, device manufacturer and model of the originating device.
    /// Reported in the format: OS Family/OS Version+ (Device Manufacturer/Device Model+)
    /// </summary>
    [AntiFraudHeader("Gov-Client-User-Agent")]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.BATCH_PROCESS_DIRECT)]
    [ConnectionMethod(ConnectionMethod.OTHER_DIRECT)]
    [ConnectionMethod(ConnectionMethod.OTHER_VIA_SERVER)]
    public UserAgent UserAgent { get; set; }

    /// <summary>Gets or sets the accounts the user holds.</summary>
    [AntiFraudHeader("Gov-Client-User-IDs", true)]
    [ConnectionMethod(true)]
    public Dictionary<string, string> UserIDs { get; set; }

    /// <summary>
    /// Gets or sets a list that details hops over the internet between services that terminate TLS.
    /// </summary>
    [AntiFraudHeader("Gov-Vendor-Forwarded")]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.OTHER_VIA_SERVER)]
    public IEnumerable<Forwarded> VendorForwarded { get; set; }

    /// <summary>
    /// Gets or sets the hashed licence keys relating to the software vendor
    /// initiating the API request on the originating device.
    /// </summary>
    [AntiFraudHeader("Gov-Vendor-License-IDs", true)]
    [ConnectionMethod(true)]
    public Dictionary<string, string> VendorLicenceIDs { get; set; }

    /// <summary>Gets or sets the public IP address (IPv4 or IPv6) from which the originating device makes the request.</summary>
    [AntiFraudHeader("Gov-Vendor-Public-IP")]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.OTHER_VIA_SERVER)]
    public IPAddress VendorPublicAddress { get; set; }

    /// <summary>Gets or sets the list of software versions involved in handling the request.</summary>
    [AntiFraudHeader("Gov-Vendor-Version")]
    [ConnectionMethod(true)]
    public Dictionary<string, string> VendorVersion { get; set; }

    /// <summary>
    /// Gets or sets the number of pixels of the window on the originating device in which the
    /// user initiated (directly or indirectly) the API call to HMRC.
    /// </summary>
    [AntiFraudHeader("Gov-Client-Window-Size", true)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_DIRECT)]
    [ConnectionMethod(ConnectionMethod.MOBILE_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.DESKTOP_APP_VIA_SERVER)]
    [ConnectionMethod(ConnectionMethod.WEB_APP_VIA_SERVER)]
    public Size WindowSize { get; set; }
    #endregion

    #region Methods
    /// <summary>Returns a collection containing the properties that are required by the specified <see cref="ConnectionMethod"/>.</summary>
    public static IEnumerable<PropertyInfo> GetPropertiesForMethod(ConnectionMethod method) {
      return typeof(AntiFraud)
        .GetProperties()
        .Where(x => {
          return (x.GetCustomAttribute<AntiFraudHeaderAttribute>() != null)
          && (
          x.Name == nameof(ConnectionMethod)
          || x.GetCustomAttributes<ConnectionMethodAttribute>().Any(a => a.IsRequired(method))
          );
        });
    }

    /// <summary>Returns a collection of all the anti fraud headers.</summary>
    public Dictionary<string, string> GetAntiFraudHeaders() {
      var errors = new List<string>();
      var headers = GetAntiFraudHeaders(errors);

      if (errors.Any()) {
        throw new AntiFraudException($"{errors.Count()} validation errors were found.") {
          Errors = errors
        };
      }

      return headers;
    }

    private Dictionary<string, string> GetAntiFraudHeaders(List<string> errors) {
      var properties = GetPropertiesForMethod(ConnectionMethod);

      var headers = new Dictionary<string, string>();

      foreach (var item in properties) {
        var afHeader = item.GetCustomAttribute<AntiFraudHeaderAttribute>();
        var value = item.GetValue(this);

        bool isEmpty;
        if (value == null) {
          isEmpty = true;
        } else if (value is IEnumerable list) {
          isEmpty = !list.Any();
        } else if ("".Equals(value)) {
          isEmpty = true;
        } else {
          isEmpty = false;
        }

        if (isEmpty && !afHeader.AllowEmpty) {
          errors.Add($"{item.Name} cannot be empty.");
          continue;
        }

        string headerValue;
        if (value is string str) {
          headerValue = str; // string are enumerable so catch them here

        } else if (value is IDictionary dict) {
          var sb = new StringBuilder();
          foreach (var key in dict.Keys) {
            if (sb.Length != 0) sb.Append("&");
            sb.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode($"{dict[key]}"));
          }
          headerValue = sb.ToString();

        } else if (value is IEnumerable list) {
          var sb = new StringBuilder();
          foreach (var o in list) {
            if (sb.Length != 0) sb.Append(",");
            if (o == null) {
              errors.Add($"{item.Name} contains a null value.");

            } else if (o is IAntiFraudValue val) {
              sb.Append(val.GetHeaderValue());
            } else {
              sb.Append($"{o}");
            }
          }
          headerValue = sb.ToString();

        } else if (value is IAntiFraudValue val) {
          headerValue = val.GetHeaderValue();

        } else if (value is TimeZoneInfo tz) {
          var symbol = tz.BaseUtcOffset.TotalHours > 0 ? "+" : "-";
          headerValue = $"UTC{symbol}{tz.BaseUtcOffset:hh\\:mm}";

        } else {
          headerValue = $"{value}";

        }

        if (headerValue != "") {
          headers.Add(afHeader.HeaderName, headerValue);
        }
      }

      return headers;
    }

    /// <summary>Populates the <see cref="LocalIPs"/> property with all the local IP addresses.</summary>
    public void PopulateLocalIPs() {
      LocalIPs = NetworkInterface.GetAllNetworkInterfaces().GetAllAddresses();
    }

    /// <summary>Populates the <see cref="MACAddresses"/> property with all the local MAC addresses.</summary>
    public void PopulateMACAddresses() {
      var foo = NetworkInterface.GetAllNetworkInterfaces().GetAllMACAddresses();
      MACAddresses = NetworkInterface.GetAllNetworkInterfaces().GetAllMACAddresses().Select(m => m.FormatMAC());
    }

#if NET452
    /// <summary>Populates the <see cref="Screens"/> property with all the screens in the system.</summary>
    public void PopulateScreens() {
      Screens = System.Windows.Forms.Screen.AllScreens
        .Select(s => {
          return new Screen() {
            ColourDepth = s.BitsPerPixel,
            ScalingFactor = 1,
            Size = new Size(s.Bounds.Size)
          };
        });
    }
#endif

    /// <summary>Populates the <see cref="UserAgent"/> property with the local operating system info.</summary>
    public void PopulateUserAgent() {
      var os = System.Environment.OSVersion;
      UserAgent = new UserAgent() {
        OSFamily = $"{os.Platform}",
        OSVersion = os.VersionString,
      };
    }

    /// <summary>Returns a flag indicating whether the anti fraud headers are valid.</summary>
    public bool Validate() {
      return Validate(out string[] errors);
    }

    /// <summary>
    /// Returns a flag indicating whether the anti fraud headers are valid.
    /// Deprecated: Use the <see cref="Validate(out string[])"/> method instead.
    /// </summary>
    /// <param name="errors">The list of errors to populate.</param>
    [Obsolete]
    public bool Validate(List<string> errors) {
      GetAntiFraudHeaders(errors);
      return !errors.Any();
    }

    /// <summary>Returns a flag indicating whether the anti fraud headers are valid.</summary>
    /// <param name="errors">The list of errors to populate.</param>
    public bool Validate(out string[] errors) {
      var list = new List<string>();
      GetAntiFraudHeaders(list);
      errors = list.ToArray();
      return !list.Any();
    }
    #endregion
  }
}
