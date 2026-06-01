using System.Collections.Generic;

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
    internal static FraudPreventionHeader GetMultiFactorHeader(this IMultiFactor source) =>
      new FraudPreventionHeader("Gov-Client-Multi-Factor", true, source.MultiFactor);
  }
}
