using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.FraudPrevention;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd {
  /// <summary>Service that exposes Business Details (MTD) functions, supporting dependency injection.</summary>
  public class BusinessDetailsMtdService : HmrcServiceBase, IRequiresFraudPrevention {
    /// <inheritdoc/>
    public override string Description => "Business Details API to retrieve detailed information about a customer's self-employment or property business.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "individuals/business/details";

    /// <inheritdoc/>
    public override string Name => "Business Details (MTD) API";

    /// <inheritdoc/>
    public override string Version => "2.0";

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public BusinessDetailsMtdService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Create or amend the type of quarterly reporting period used for a business for a specific tax year.</summary>
    [Obsolete("This method is deprecated, use CreateOrAmendQuarterlyPeriodTypeAsync instead.")]
    public AmendQuarterlyPeriodTypeResponse CreateOrAmendQuarterlyPeriodType(AmendQuarterlyPeriodTypeRequest request, IFraudPrevention? fraudPreventionConfig = null) {
      return ExecuteRequest<AmendQuarterlyPeriodTypeResponse>(request, fraudPreventionConfig);
    }

    /// <summary>Asynchronously create or amend the type of quarterly reporting period used for a business for a specific tax year.</summary>
    public async Task<AmendQuarterlyPeriodTypeResponse> CreateOrAmendQuarterlyPeriodTypeAsync(AmendQuarterlyPeriodTypeRequest request, IFraudPrevention? fraudPreventionConfig, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<AmendQuarterlyPeriodTypeResponse>(request, fraudPreventionConfig, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously create or amend the type of quarterly reporting period used for a business for a specific tax year.</summary>
    public Task<AmendQuarterlyPeriodTypeResponse> CreateOrAmendQuarterlyPeriodTypeAsync(AmendQuarterlyPeriodTypeRequest request, CancellationToken cancellationToken = default) {
      return CreateOrAmendQuarterlyPeriodTypeAsync(request, null, cancellationToken);
    }

    /// <summary>Gets additional information for a specific business income source.</summary>
    [Obsolete("This method is deprecated, use GetBusinessDetailsAsync instead.")]
    public GetBusinessDetailsResponse GetBusinessDetails(GetBusinessDetailsRequest request, IFraudPrevention? fraudPreventionConfig = null) {
      return ExecuteRequest<GetBusinessDetailsResponse>(request, fraudPreventionConfig);
    }

    /// <summary>Asynchronously gets additional information for a specific business income source.</summary>
    public async Task<GetBusinessDetailsResponse> GetBusinessDetailsAsync(GetBusinessDetailsRequest request, IFraudPrevention? fraudPreventionConfig, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<GetBusinessDetailsResponse>(request, fraudPreventionConfig, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously gets additional information for a specific business income source.</summary>
    public Task<GetBusinessDetailsResponse> GetBusinessDetailsAsync(GetBusinessDetailsRequest request, CancellationToken cancellationToken = default) {
      return GetBusinessDetailsAsync(request, null, cancellationToken);
    }

    /// <summary>Gets all details of a user's business income sources.</summary>
    [Obsolete("This method is deprecated, use ListBusinessDetailsAsync instead.")]
    public ListBusinessDetailsResponse ListBusinessDetails(ListBusinessDetailsRequest request, IFraudPrevention? fraudPreventionConfig = null) {
      return ExecuteRequest<ListBusinessDetailsResponse>(request, fraudPreventionConfig);
    }

    /// <summary>Asynchronously gets all details of a user's business income sources.</summary>
    public async Task<ListBusinessDetailsResponse> ListBusinessDetailsAsync(ListBusinessDetailsRequest request, IFraudPrevention? fraudPreventionConfig, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<ListBusinessDetailsResponse>(request, fraudPreventionConfig, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously gets all details of a user's business income sources.</summary>
    public Task<ListBusinessDetailsResponse> ListBusinessDetailsAsync(ListBusinessDetailsRequest request, CancellationToken cancellationToken = default) {
      return ListBusinessDetailsAsync(request, null, cancellationToken);
    }
  }
}
