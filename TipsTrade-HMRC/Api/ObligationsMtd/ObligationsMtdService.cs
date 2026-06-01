using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using TipsTrade.HMRC.Api.OAuth;

namespace TipsTrade.HMRC.Api.ObligationsMtd {
  /// <summary>Service that exposes Obligations (MTD) functions, supporting dependency injection.</summary>
  public class ObligationsMtdService : HmrcServiceBase, IRequiresAntiFraud {
    /// <inheritdoc/>
    public override string Description => "Retrieve obligations for a user's business income sources.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "obligations/details";

    /// <inheritdoc/>
    public override string Name => "Obligations (MTD) API";

    /// <inheritdoc/>
    public override string Version => "3.0";

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public ObligationsMtdService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Retrieve obligations for a user's business income sources.</summary>
    public GetObligationsResponse GetIncomeAndExpenditureObligations(GetObligationsRequest request) {
      return ExecuteRequest<GetObligationsResponse>(request);
    }

    /// <summary>Asynchronously retrieve obligations for a user's business income sources.</summary>
    public async Task<GetObligationsResponse> GetIncomeAndExpenditureObligationsAsync(GetObligationsRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<GetObligationsResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve the final declaration obligations for a customer's Income Tax account.</summary>
    public GetFinalObligationsResponse GetFinalObligations(GetFinalObligationsRequest request) {
      return ExecuteRequest<GetFinalObligationsResponse>(request);
    }

    /// <summary>Asynchronously retrieve the final declaration obligations for a customer's Income Tax account.</summary>
    public async Task<GetFinalObligationsResponse> GetFinalObligationsAsync(GetFinalObligationsRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<GetFinalObligationsResponse>(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
