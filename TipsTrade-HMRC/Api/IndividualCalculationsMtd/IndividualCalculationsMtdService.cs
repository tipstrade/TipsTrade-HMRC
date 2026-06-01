using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.FraudPrevention;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd {
  /// <summary>Service that exposes Individual Calculations (MTD) functions, supporting dependency injection.</summary>
  public class IndividualCalculationsMtdService : HmrcServiceBase, IRequiresFraudPrevention {
    /// <inheritdoc/>
    public override string Description => "Trigger, list, retrieve and submit a customer's self-assessment tax calculation.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "individuals/calculations";

    /// <inheritdoc/>
    public override string Name => "Individual Calculations (MTD) API";

    /// <inheritdoc/>
    public override string Version => "8.0";

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public IndividualCalculationsMtdService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>List Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
    [Obsolete("This method is deprecated. Please use ListSelfAssessmentCalculationsAsync instead.")]
    public ListSelfAssessmentCalculationsResponse ListSelfAssessmentCalculations(ListSelfAssessmentCalculationsRequest request) {
      return ExecuteRequest<ListSelfAssessmentCalculationsResponse>(request);
    }

    /// <summary>Asynchronously list Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
    public async Task<ListSelfAssessmentCalculationsResponse> ListSelfAssessmentCalculationsAsync(ListSelfAssessmentCalculationsRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<ListSelfAssessmentCalculationsResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    [Obsolete("This method is deprecated. Please use RetrieveSelfAssessmentCalculationAsync instead.")]
    public RetrieveSelfAssessmentCalculationResponse RetrieveSelfAssessmentCalculation(RetrieveSelfAssessmentCalculationRequest request) {
      return ExecuteRequest<RetrieveSelfAssessmentCalculationResponse>(request);
    }

    /// <summary>Asynchronously retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    public async Task<RetrieveSelfAssessmentCalculationResponse> RetrieveSelfAssessmentCalculationAsync(RetrieveSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<RetrieveSelfAssessmentCalculationResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Submit a final declaration for a tax year.</summary>
    [Obsolete("This method is deprecated. Please use SubmitFinalAssessmentAsync instead.")]
    public SubmitFinalAssessmentResponse SubmitFinalAssessment(SubmitFinalAssessmentRequest request) {
      return ExecuteRequest<SubmitFinalAssessmentResponse>(request);
    }

    /// <summary>Asynchronously submit a final declaration for a tax year.</summary>
    public async Task<SubmitFinalAssessmentResponse> SubmitFinalAssessmentAsync(SubmitFinalAssessmentRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<SubmitFinalAssessmentResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Triggers a self assessment tax calculation for a given tax year.</summary>
    [Obsolete("This method is deprecated. Please use TriggerCalculationAsync instead.")]
    public TriggerSelfAssessmentCalculationResponse TriggerCalculation(TriggerSelfAssessmentCalculationRequest request) {
      return ExecuteRequest<TriggerSelfAssessmentCalculationResponse>(request);
    }

    /// <summary>Asynchronously triggers a self assessment tax calculation for a given tax year.</summary>
    public async Task<TriggerSelfAssessmentCalculationResponse> TriggerCalculationAsync(TriggerSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<TriggerSelfAssessmentCalculationResponse>(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
