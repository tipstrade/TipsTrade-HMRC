using RestSharp;
using System.Collections.Generic;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.FraudPrevention {
  /// <summary>Base interface that all fraud prevention implementations must satisfy.</summary>
  public interface IFraudPrevention : IConnectionMethod {
    /// <summary>Returns all fraud prevention headers for this connection method.</summary>
    IEnumerable<(string Name, string Value)> GetHeaders();
  }

  internal static class FraudPreventionExtensions {
    /// <summary>
    /// Adds all fraud prevention headers to the given request.
    /// </summary>
    /// <param name="source">The fraud prevention implementation.</param>
    /// <param name="request">The request to which headers will be added.</param>
    /// <returns>The request with added headers.</returns>
    internal static RestRequest AddHeadersToRequest(this IFraudPrevention source, RestRequest request) {
      foreach (var header in source.GetHeaders()) {
        request.AddHeader(header.Name, header.Value);
      }

      return request;
    }
  }
}
