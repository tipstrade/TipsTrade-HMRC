using System.Collections.Generic;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>
  /// Represents a request model used to create a test user in the HMRC sandbox.
  /// </summary>
  /// <remarks>
  /// Implementations provide the list of service names that the created test user
  /// should be enrolled for. This interface is typically serialized and sent to
  /// the Create Test User endpoint.
  /// </remarks>
  public interface ICreateTestUserRequest {
    /// <summary>
    /// Gets or sets the list of services that the test user should be enrolled for.
    /// </summary>
    /// <value>
    /// A <see cref="List{String}"/> containing the service names (as strings).
    /// Each entry typically corresponds to an HMRC service identifier the user
    /// should have access to (for example, "PAYE" or "MTD-VAT").
    /// </value>
    List<string> ServiceNames { get; set; }
  }

  /// <summary>
  /// Represents a typed request model that provides a list of service names and
  /// specifies the expected result type returned when creating a user.
  /// </summary>
  /// <typeparam name="TResult">
  /// The type of result returned by the create user operation. Must derive from
  /// <see cref="UserResultBase"/>.
  /// </typeparam>
  public interface ICreateTestUserRequest<TResult> : ICreateTestUserRequest where TResult : UserResultBase {
  }
}
