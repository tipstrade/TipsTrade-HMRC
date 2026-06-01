using System;
using System.Net;
using System.Web;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Represents an object that contains information on hops over the internet that terminate TLS.</summary>
  public class Forwarded : IFraudPreventionValue   {
    /// <summary>Gets or sets the server’s public IP address where it received the request.</summary>
    public IPAddress By { get; set; }

    /// <summary>Gets or sets requestor’s public IP address from which the vendor received the request.</summary>
    public IPAddress For { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Forwarded"/> class with the specified IP addresses.
    /// </summary>
    /// <param name="by">The server’s public IP address where it received the request.</param>
    /// <param name="for">The requestor’s public IP address from which the vendor received the request.</param>
    public Forwarded(IPAddress by, IPAddress @for) {
      if (by == null) {
        throw new ArgumentNullException(nameof(by));
      } else if (@for == null) {
        throw new ArgumentNullException(nameof(@for));
      }

      By = by;
      For = @for;
    }


    /// <summary>Returns a string that contains the fraud prevention header value.</summary>
    public string GetHeaderValue() {
      var by = HttpUtility.UrlEncode($"{By}");
      var @for = HttpUtility.UrlEncode($"{For}");

      return $"by={by}&for={@for}";
    }
  }
}
