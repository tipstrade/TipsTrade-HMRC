using System;
using System.Net;
using System.Web;

namespace TipsTrade.HMRC.AntiFraud {
  /// <summary>Represents an object that contains information on hops over the internet that terminate TLS.</summary>
  public class Forwarded : IAntiFraudValue {
    /// <summary>Gets or sets the server’s public IP address where it received the request.</summary>
    public IPAddress By { get; set; }

    /// <summary>Gets or sets requestor’s public IP address from which the vendor received the request.</summary>
    public IPAddress For { get; set; }

    /// <summary>Retuns a string that contains the anti fraud header value.</summary>
    public string GetHeaderValue() {
      if (By == null) throw new InvalidOperationException($"The {nameof(By)} property cannot be null.");
      if (For == null) throw new InvalidOperationException($"The {nameof(For)} property cannot be null.");

      return $"by={HttpUtility.UrlEncode(By.ToString())}&for={HttpUtility.UrlEncode(For.ToString())}";
    }
  }
}
