using System.Collections.Generic;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.FraudPrevention {
  /// <summary>Base interface that all fraud prevention implementations must satisfy.</summary>
  public interface IFraudPrevention : IConnectionMethod {
    /// <summary>Returns all fraud prevention headers for this connection method.</summary>
    IEnumerable<FraudPreventionHeader> GetHeaders();
  }
}
