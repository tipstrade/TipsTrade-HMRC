using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd {
  /// <summary>Service that exposes Self Employment Business (MTD) functions, supporting dependency injection.</summary>
  public class SelfEmploymentBusinessMtdService : HmrcServiceBase, IRequiresAntiFraud {
    /// <inheritdoc/>
    public override string Description => "Create or amend a self-employment annual summary for a tax year.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "individuals/business/self-employment";

    /// <inheritdoc/>
    public override string Name => "Self Employment Business (MTD) API";

    /// <inheritdoc/>
    public override string Version => "5.0";

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public SelfEmploymentBusinessMtdService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Submit or amend the cumulative period income and expenses for a self-employment business.</summary>
    [Obsolete("Use CreateOrAmendCumulativePeriodSummaryAsync instead. Synchronous methods may cause deadlocks.")]
    public AmendCumulativePeriodSummaryResponse CreateOrAmendCumulativePeriodSummary(AmendCumulativePeriodSummaryRequest request) {
      return ExecuteRequest<AmendCumulativePeriodSummaryResponse>(request);
    }

    /// <summary>Asynchronously submit or amend the cumulative period income and expenses for a self-employment business.</summary>
    public async Task<AmendCumulativePeriodSummaryResponse> CreateOrAmendCumulativePeriodSummaryAsync(AmendCumulativePeriodSummaryRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<AmendCumulativePeriodSummaryResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve the cumulative period income and expenses for a self-employment business.</summary>
    [Obsolete("Use GetCumulativePeriodSummaryAsync instead. Synchronous methods may cause deadlocks.")]
    public GetCumulativePeriodSummaryResponse GetCumulativePeriodSummary(GetCumulativePeriodSummaryRequest request) {
      return ExecuteRequest<GetCumulativePeriodSummaryResponse>(request);
    }

    /// <summary>Asynchronously retrieve the cumulative period income and expenses for a self-employment business.</summary>
    public async Task<GetCumulativePeriodSummaryResponse> GetCumulativePeriodSummaryAsync(GetCumulativePeriodSummaryRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<GetCumulativePeriodSummaryResponse>(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
