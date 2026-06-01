using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.OAuth;

namespace TipsTrade.HMRC.Api.CreateTestUser {
  /// <summary>Service that exposes Create Test User functions, supporting dependency injection.</summary>
  public class CreateTestUserService : HmrcServiceBase {
    /// <inheritdoc/>
    public override string Description => "An API to create test users for testing in our sandbox with user-restricted endpoints.";

    /// <inheritdoc/>
    public override bool IsStable => false;

    /// <inheritdoc/>
    public override string Location => "create-test-user";

    /// <inheritdoc/>
    public override string Name => "Create Test User API";

    /// <inheritdoc/>
    public override string Version => "1.0";

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public CreateTestUserService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Executes a create test user request synchronously.</summary>
    public TResult CreateUser<TResult>(ICreateTestUserRequest<TResult> request) where TResult : UserResultBase, new() {
      if (request is IApiRequest apiRequest) {
        return ExecuteRequest<TResult>(apiRequest);
      }

      throw new ArgumentException($"The request must implement IApiRequest to be executed. Request type: {request.GetType().FullName}", nameof(request));
    }

    /// <summary>Executes a create test user request asynchronously.</summary>
    public async Task<TResult> CreateUserAsync<TResult>(ICreateTestUserRequest<TResult> request, CancellationToken cancellationToken = default) where TResult : UserResultBase, new() {
      if (request is IApiRequest apiRequest) {
        return await ExecuteRequestAsync<TResult>(apiRequest, cancellationToken).ConfigureAwait(false);
      }

      throw new ArgumentException($"The request must implement IApiRequest to be executed. Request type: {request.GetType().FullName}", nameof(request));
    }
  }
}
