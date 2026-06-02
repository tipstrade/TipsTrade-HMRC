using System.Collections.Generic;
using System.Linq;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Multi-Factor header.</summary>
  public interface IMultiFactor {
    /// <summary>
    /// Gets or sets a list of key-value data structures containing details of the multi-factor
    /// authentication (MFA) statuses related to the API call.
    /// </summary>
    IEnumerable<MultiFactor>? MultiFactor { get; set; }
  }

  internal static class MultiFactorExtensions {
    internal static (string Name, string Value) GetMultiFactor(this IMultiFactor source) {
      var value = source.MultiFactor == null ? "" : string.Join(",", source.MultiFactor.Select(x => x.GetHeaderValue()));

      return ("Gov-Client-Multi-Factor", value);
    }
  }
}
