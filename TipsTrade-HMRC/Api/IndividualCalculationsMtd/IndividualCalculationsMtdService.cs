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
    public ListSelfAssessmentCalculationsResponse ListSelfAssessmentCalculations(ListSelfAssessmentCalculationsRequest request, IFraudPrevention? fraudPrevention = null) {
      return ExecuteRequest<ListSelfAssessmentCalculationsResponse>(request, fraudPrevention);
    }

    /// <summary>Asynchronously list Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
    public async Task<ListSelfAssessmentCalculationsResponse> ListSelfAssessmentCalculationsAsync(ListSelfAssessmentCalculationsRequest request, IFraudPrevention? fraudPrevention, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<ListSelfAssessmentCalculationsResponse>(request, fraudPrevention, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously list Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
    public Task<ListSelfAssessmentCalculationsResponse> ListSelfAssessmentCalculationsAsync(ListSelfAssessmentCalculationsRequest request, CancellationToken cancellationToken = default) {
      return ListSelfAssessmentCalculationsAsync(request, null, cancellationToken);
    }

    /// <summary>Retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    [Obsolete("This method is deprecated. Please use RetrieveSelfAssessmentCalculationAsync instead.")]
    public RetrieveSelfAssessmentCalculationResponse RetrieveSelfAssessmentCalculation(RetrieveSelfAssessmentCalculationRequest request, IFraudPrevention? fraudPrevention = null) {
      return ExecuteRequest<RetrieveSelfAssessmentCalculationResponse>(request, fraudPrevention);
    }

    /// <summary>Asynchronously retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    public async Task<RetrieveSelfAssessmentCalculationResponse> RetrieveSelfAssessmentCalculationAsync(RetrieveSelfAssessmentCalculationRequest request, IFraudPrevention? fraudPrevention, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<RetrieveSelfAssessmentCalculationResponse>(request, fraudPrevention, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    public Task<RetrieveSelfAssessmentCalculationResponse> RetrieveSelfAssessmentCalculationAsync(RetrieveSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return RetrieveSelfAssessmentCalculationAsync(request, null, cancellationToken);
    }

    /// <summary>Submit a final declaration for a tax year.</summary>
    [Obsolete("This method is deprecated. Please use SubmitFinalAssessmentAsync instead.")]
    public SubmitFinalAssessmentResponse SubmitFinalAssessment(SubmitFinalAssessmentRequest request, IFraudPrevention? fraudPrevention = null) {
      return ExecuteRequest<SubmitFinalAssessmentResponse>(request, fraudPrevention);
    }

    /// <summary>Asynchronously submit a final declaration for a tax year.</summary>
    public async Task<SubmitFinalAssessmentResponse> SubmitFinalAssessmentAsync(SubmitFinalAssessmentRequest request, IFraudPrevention? fraudPrevention, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<SubmitFinalAssessmentResponse>(request, fraudPrevention, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously submit a final declaration for a tax year.</summary>
    public Task<SubmitFinalAssessmentResponse> SubmitFinalAssessmentAsync(SubmitFinalAssessmentRequest request, CancellationToken cancellationToken = default) {
      return SubmitFinalAssessmentAsync(request, null, cancellationToken);
    }

    /// <summary>Triggers a self assessment tax calculation for a given tax year.</summary>
    [Obsolete("This method is deprecated. Please use TriggerCalculationAsync instead.")]
    public TriggerSelfAssessmentCalculationResponse TriggerCalculation(TriggerSelfAssessmentCalculationRequest request, IFraudPrevention? fraudPrevention = null) {
      return ExecuteRequest<TriggerSelfAssessmentCalculationResponse>(request, fraudPrevention);
    }

    /// <summary>Asynchronously triggers a self assessment tax calculation for a given tax year.</summary>
    public async Task<TriggerSelfAssessmentCalculationResponse> TriggerCalculationAsync(TriggerSelfAssessmentCalculationRequest request, IFraudPrevention? fraudPrevention, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<TriggerSelfAssessmentCalculationResponse>(request, fraudPrevention, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously triggers a self assessment tax calculation for a given tax year.</summary>
    public Task<TriggerSelfAssessmentCalculationResponse> TriggerCalculationAsync(TriggerSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return TriggerCalculationAsync(request, null, cancellationToken);
    }
  }
}
