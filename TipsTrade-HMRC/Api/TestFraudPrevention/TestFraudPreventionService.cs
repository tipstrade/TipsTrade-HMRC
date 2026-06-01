using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Api.TestFraudPrevention.Model;
using TipsTrade.HMRC.FraudPrevention;

namespace TipsTrade.HMRC.Api.TestFraudPrevention {
  /// <summary>Service that exposes Test Fraud Prevention Header functions, supporting dependency injection.</summary>
  public class TestFraudPreventionService : HmrcServiceBase, IRequiresFraudPrevention {
    /// <inheritdoc/>
    public override string Description => "An API for testing Fraud Prevention headers.";

    /// <inheritdoc/>
    public override bool IsStable => false;

    /// <inheritdoc/>
    public override string Location => "test/fraud-prevention-headers";

    /// <inheritdoc/>
    public override string Name => "Test Fraud Prevention Headers API";

    /// <inheritdoc/>
    public override string Version => "1.0";

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public TestFraudPreventionService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Submits feedback about the fraud prevention headers sent with an API request.</summary>
    [Obsolete("Use GetFeedbackAsync instead. Synchronous methods may cause deadlocks.")]
    public FeedbackResult GetFeedback(string api, ConnectionMethod connectionMethod) {
      return ExecuteRequest<FeedbackResult>(new FeedbackRequest { Api = api, ConnectionMethod = connectionMethod });
    }

    /// <summary>Asynchronously submits feedback about the fraud prevention headers sent with an API request.</summary>
    public async Task<FeedbackResult> GetFeedbackAsync(string api, ConnectionMethod connectionMethod, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<FeedbackResult>(
        new FeedbackRequest { Api = api, ConnectionMethod = connectionMethod },
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Validates fraud prevention headers submitted with this HTTP request.</summary>
    [Obsolete("Use ValidateAsync instead. Synchronous methods may cause deadlocks.")]
    public ValidateResult Validate() {
      return ExecuteRequest<ValidateResult>(new ValidateRequest());
    }

    /// <summary>Validates fraud prevention headers submitted with this HTTP request asynchronously.</summary>
    public async Task<ValidateResult> ValidateAsync(CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<ValidateResult>(
        new ValidateRequest(),
        cancellationToken).ConfigureAwait(false);
    }
  }
}
