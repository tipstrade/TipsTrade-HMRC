using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd {
  /// <summary>Service that exposes Individual Calculations (MTD) functions, supporting dependency injection.</summary>
  public class IndividualCalculationsMtdService : HmrcServiceBase, IRequiresAntiFraud {
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

    /// <summary>Initialises a new instance using dependency-injected options.</summary>
    public IndividualCalculationsMtdService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, ApplicationTokenCache applicationTokenCache) : base(options, httpClientFactory, applicationTokenCache) { }

    /// <summary>Initialises a new instance using a plain <see cref="HmrcOptions"/> object.</summary>
    public IndividualCalculationsMtdService(HmrcOptions options, IHttpClientFactory httpClientFactory, ApplicationTokenCache applicationTokenCache) : base(options, httpClientFactory, applicationTokenCache) { }

    /// <summary>List Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
    public ListSelfAssessmentCalculationsResponse ListSelfAssessmentCalculations(ListSelfAssessmentCalculationsRequest request) {
      return this.ExecuteRequest<ListSelfAssessmentCalculationsResponse>(request);
    }

    /// <summary>Asynchronously list Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
    public async Task<ListSelfAssessmentCalculationsResponse> ListSelfAssessmentCalculationsAsync(ListSelfAssessmentCalculationsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<ListSelfAssessmentCalculationsResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    public RetrieveSelfAssessmentCalculationResponse RetrieveSelfAssessmentCalculation(RetrieveSelfAssessmentCalculationRequest request) {
      return this.ExecuteRequest<RetrieveSelfAssessmentCalculationResponse>(request);
    }

    /// <summary>Asynchronously retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    public async Task<RetrieveSelfAssessmentCalculationResponse> RetrieveSelfAssessmentCalculationAsync(RetrieveSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<RetrieveSelfAssessmentCalculationResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Submit a final declaration for a tax year.</summary>
    public SubmitFinalAssessmentResponse SubmitFinalAssessment(SubmitFinalAssessmentRequest request) {
      return this.ExecuteRequest<SubmitFinalAssessmentResponse>(request);
    }

    /// <summary>Asynchronously submit a final declaration for a tax year.</summary>
    public async Task<SubmitFinalAssessmentResponse> SubmitFinalAssessmentAsync(SubmitFinalAssessmentRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<SubmitFinalAssessmentResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Triggers a self assessment tax calculation for a given tax year.</summary>
    public TriggerSelfAssessmentCalculationResponse TriggerCalculation(TriggerSelfAssessmentCalculationRequest request) {
      return this.ExecuteRequest<TriggerSelfAssessmentCalculationResponse>(request);
    }

    /// <summary>Asynchronously triggers a self assessment tax calculation for a given tax year.</summary>
    public async Task<TriggerSelfAssessmentCalculationResponse> TriggerCalculationAsync(TriggerSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<TriggerSelfAssessmentCalculationResponse>(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
