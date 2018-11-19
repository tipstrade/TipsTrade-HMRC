using System.Collections.Generic;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that all Api responses must inherit from.</summary>
  public interface IApiResponse<T>  {
    /// <summary>The value of the response.</summary>
    T Value { get; set; }
  }

  /// <summary>Represents a model that all Api responses must inherit from.</summary>
  public interface IApiListResponse<T> : IApiResponse<IEnumerable<T>> {
  }
}
