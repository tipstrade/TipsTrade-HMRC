using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.Model;

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
    /// Executes a create test user request synchronously and returns the resulting user information.
    /// </summary>
    /// <typeparam name="TResult">
    /// The concrete result type expected from the request. Must derive from <see cref="UserResultBase"/>.
    /// </typeparam>
    /// <param name="request">
    /// The create test user request to execute. The request is required to implement <see cref="IApiRequest"/>;
    /// when this is the case it will be forwarded to <c>ExecuteRequest{TResult}</c>.
    /// </param>
    /// <returns>
    /// An instance of <typeparamref name="TResult"/> containing the user information returned by the API.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied <paramref name="request"/> does not implement <see cref="IApiRequest"/>.
    /// </exception>
    public TResult CreateUser<TResult>(ICreateTestUserRequest<TResult> request) where TResult : UserResultBase {
      // IApiRequest is internal, so we can't enforce this constraint at compile time on the ICreateTestUserRequest interface.
      if (request is IApiRequest apiRequest) {
        return this.ExecuteRequest<TResult>(apiRequest);
      }

      throw new ArgumentException($"The request must implement IApiRequest to be executed. Request type: {request.GetType().FullName}", nameof(request));
    }

    /// <summary>
    /// Executes a create test user request asynchronously and returns the resulting user information.
    /// </summary>
    /// <typeparam name="TResult">
    /// The concrete result type expected from the request. Must derive from <see cref="UserResultBase"/>.
    /// </typeparam>
    /// <param name="request">
    /// The create test user request to execute. The request is required to implement <see cref="IApiRequest"/>;
    /// when this is the case it will be forwarded to <c>ExecuteRequestAsync{TResult}</c>.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains an instance
    /// of <typeparamref name="TResult"/> with the user information returned by the API.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied <paramref name="request"/> does not implement <see cref="IApiRequest"/>.
    /// </exception>
    public async Task<TResult> CreateUserAsync<TResult>(ICreateTestUserRequest<TResult> request, CancellationToken cancellationToken = default) where TResult : UserResultBase {
      // IApiRequest is internal, so we can't enforce this constraint at compile time on the ICreateTestUserRequest interface.
      if (request is IApiRequest apiRequest) {
        return await this.ExecuteRequestAsync<TResult>(apiRequest, cancellationToken).ConfigureAwait(false);
      }

      throw new ArgumentException($"The request must implement IApiRequest to be executed. Request type: {request.GetType().FullName}", nameof(request));
    }
    #endregion
  }
}
