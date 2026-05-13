using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.CreateTestUser.Model;

namespace TipsTrade.HMRC.Api.CreateTestUser {
  /// <summary>
  /// The API that exposes Create Test User functions.
  /// </summary>
  /// <remarks>
  /// Provides methods to create test users (agent, individual, organisation) for use in the sandbox environment.
  /// Overloads are provided for synchronous and asynchronous usage and a dispatching overload accepts an
  /// <see cref="ICreateTestUserRequest"/> which will forward the call to the corresponding concrete overload
  /// based on the runtime type of the request.
  /// </remarks>
  public class CreateTestUserApi : IApi, IClient {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>
    public string Description => "An API to create test users for testing in our sandbox with user-restricted endpoints.";

    /// <inheritdoc/>
    public bool IsStable => false;

    /// <inheritdoc/>
    public string Location => "create-test-user";

    /// <inheritdoc/>
    public string Name => "Create Test User API";

    /// <inheritdoc/>
    public string Version => "1.0";
    #endregion

    #region API Methods
    /// <summary>
    /// Creates a test user by dispatching to the concrete overload that matches the runtime type of the supplied request.
    /// </summary>
    /// <param name="request">The create test user request instance; must implement <see cref="ICreateTestUserRequest"/>.</param>
    /// <returns>
    /// A <see cref="UserResultBase"/> instance representing the created test user. The concrete runtime type will
    /// be the appropriate result type for the supplied request (for example <see cref="AgentResult"/>,
    /// <see cref="IndividualResult"/> or <see cref="OrganisationResult"/>).
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when no matching CreateUser overload is found for the runtime type of <paramref name="request"/>.
    /// This can occur if a new request type is supplied but no corresponding CreateUser overload exists.
    /// </exception>
    public UserResultBase CreateUser(ICreateTestUserRequest request) {
      var method = GetType().GetMethods().Where(t => {
        return nameof(CreateUser).Equals(t.Name) && t.GetParameters().First().ParameterType == request.GetType();
      }).First();

      return method.Invoke(this, new object[] { request }) as UserResultBase;
    }

    /// <summary>
    /// Creates a test agent user with the specified services.
    /// </summary>
    /// <param name="request">The <see cref="CreateAgentRequest"/> describing the services and options for the agent user to create.</param>
    /// <returns>An <see cref="AgentResult"/> containing details of the created agent test user.</returns>
    public AgentResult CreateUser(CreateAgentRequest request) {
      return this.ExecuteRequest<AgentResult>(request);
    }

    /// <summary>
    /// Creates a test individual user with the specified services.
    /// </summary>
    /// <param name="request">The <see cref="CreateIndividualRequest"/> describing the services and options for the individual user to create.</param>
    /// <returns>An <see cref="IndividualResult"/> containing details of the created individual test user.</returns>
    public IndividualResult CreateUser(CreateIndividualRequest request) {
      return this.ExecuteRequest<IndividualResult>(request);
    }

    /// <summary>
    /// Creates a test organisation user with the specified services.
    /// </summary>
    /// <param name="request">The <see cref="CreateOrganisationRequest"/> describing the services and options for the organisation user to create.</param>
    /// <returns>An <see cref="OrganisationResult"/> containing details of the created organisation test user.</returns>
    public OrganisationResult CreateUser(CreateOrganisationRequest request) {
      return this.ExecuteRequest<OrganisationResult>(request);
    }

    /// <summary>
    /// Creates a test agent user with the specified services asynchronously.
    /// </summary>
    /// <param name="request">The <see cref="CreateAgentRequest"/> describing the services and options for the agent user to create.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task whose result is an <see cref="AgentResult"/> containing details of the created agent test user.</returns>
    public async Task<AgentResult> CreateUserAsync(CreateAgentRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<AgentResult>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a test individual user with the specified services asynchronously.
    /// </summary>
    /// <param name="request">The <see cref="CreateIndividualRequest"/> describing the services and options for the individual user to create.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task whose result is an <see cref="IndividualResult"/> containing details of the created individual test user.</returns>
    public async Task<IndividualResult> CreateUserAsync(CreateIndividualRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<IndividualResult>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a test organisation user with the specified services asynchronously.
    /// </summary>
    /// <param name="request">The <see cref="CreateOrganisationRequest"/> describing the services and options for the organisation user to create.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task whose result is an <see cref="OrganisationResult"/> containing details of the created organisation test user.</returns>
    public async Task<OrganisationResult> CreateUserAsync(CreateOrganisationRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<OrganisationResult>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
