using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model;
using TipsTrade.HMRC.FraudPrevention;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd {
  /// <summary>Service that exposes Self Employment Business (MTD) functions, supporting dependency injection.</summary>
  public class SelfEmploymentBusinessMtdService : HmrcServiceBase, IRequiresFraudPrevention {
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
    public SelfEmploymentBusinessMtdService(IHttpClientFactory httpClientFactory, IHmrcOptionsProvider hmrcOptionsProvider, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(httpClientFactory, hmrcOptionsProvider, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Submit or amend the cumulative period income and expenses for a self-employment business.</summary>
    [Obsolete("Use CreateOrAmendCumulativePeriodSummaryAsync instead. Synchronous methods may cause deadlocks.")]
    public AmendCumulativePeriodSummaryResponse CreateOrAmendCumulativePeriodSummary(AmendCumulativePeriodSummaryRequest request, IFraudPrevention? fraudPreventionConfig = null) {
      return ExecuteRequest<AmendCumulativePeriodSummaryResponse>(request, fraudPreventionConfig);
    }

    /// <summary>Asynchronously submit or amend the cumulative period income and expenses for a self-employment business.</summary>
    public async Task<AmendCumulativePeriodSummaryResponse> CreateOrAmendCumulativePeriodSummaryAsync(AmendCumulativePeriodSummaryRequest request, IFraudPrevention? fraudPreventionConfig, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<AmendCumulativePeriodSummaryResponse>(request, fraudPreventionConfig, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously submit or amend the cumulative period income and expenses for a self-employment business.</summary>
    public Task<AmendCumulativePeriodSummaryResponse> CreateOrAmendCumulativePeriodSummaryAsync(AmendCumulativePeriodSummaryRequest request, CancellationToken cancellationToken = default) {
      return CreateOrAmendCumulativePeriodSummaryAsync(request, null, cancellationToken);
    }

    /// <summary>Retrieve the cumulative period income and expenses for a self-employment business.</summary>
    [Obsolete("Use GetCumulativePeriodSummaryAsync instead. Synchronous methods may cause deadlocks.")]
    public GetCumulativePeriodSummaryResponse GetCumulativePeriodSummary(GetCumulativePeriodSummaryRequest request, IFraudPrevention? fraudPreventionConfig = null) {
      return ExecuteRequest<GetCumulativePeriodSummaryResponse>(request, fraudPreventionConfig);
    }

    /// <summary>Asynchronously retrieve the cumulative period income and expenses for a self-employment business.</summary>
    public async Task<GetCumulativePeriodSummaryResponse> GetCumulativePeriodSummaryAsync(GetCumulativePeriodSummaryRequest request, IFraudPrevention? fraudPreventionConfig, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<GetCumulativePeriodSummaryResponse>(request, fraudPreventionConfig, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously retrieve the cumulative period income and expenses for a self-employment business.</summary>
    public Task<GetCumulativePeriodSummaryResponse> GetCumulativePeriodSummaryAsync(GetCumulativePeriodSummaryRequest request, CancellationToken cancellationToken = default) {
      return GetCumulativePeriodSummaryAsync(request, null, cancellationToken);
    }
  }
}
