using System.Collections.Generic;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents a model that provides a list of service names.</summary>
  public interface ICreateTestUserRequest {
    /// <summary>The list of services that the user should be enrolled for.</summary>
    List<string> ServiceNames { get; set; }
  }
}
