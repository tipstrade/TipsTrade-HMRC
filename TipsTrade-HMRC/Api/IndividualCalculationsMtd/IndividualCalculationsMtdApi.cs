using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd {
  /// <summary>
  /// The API that exposes Individual Calculations (MTD) function.
  /// Provides methods to trigger, list, retrieve and submit a customer’s self-assessment tax calculations.
  /// Implements <see cref="IApi"/> and <see cref="IClient"/>.
  /// </summary>
  public class IndividualCalculationsMtdApi : IApi, IClient, IRequiresAntiFraud {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>
    public string Description => "Trigger, list, retrieve and submit a customer’s self-assessment tax calculation.";

    /// <inheritdoc/>
    public bool IsStable => true;

    /// <inheritdoc/>
    public string Location => "individuals/calculations";

    /// <inheritdoc/>
    public string Name => "Individual Calculations (MTD) API";

    /// <inheritdoc/>
    public string Version => "8.0";
    #endregion

    #region API Methods
    /// <summary>
    /// List Self Assessment tax calculations for a given National Insurance number and tax year.
    /// </summary>
    /// <param name="request">A <see cref="ListSelfAssessmentCalculationsRequest"/> containing parameters to list calculations (e.g. NI number and tax year).</param>
    /// <returns>A <see cref="ListSelfAssessmentCalculationsResponse"/> containing the list of matching calculations.</returns>
    public ListSelfAssessmentCalculationsResponse ListSelfAssessmentCalculations(ListSelfAssessmentCalculationsRequest request) {
      return this.ExecuteRequest<ListSelfAssessmentCalculationsResponse>(request);
    }

    /// <summary>
    /// List Self Assessment tax calculations for a given National Insurance number and tax year asynchronously.
    /// </summary>
    /// <param name="request">A <see cref="ListSelfAssessmentCalculationsRequest"/> containing parameters to list calculations (e.g. NI number and tax year).</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ListSelfAssessmentCalculationsResponse"/> with the list of matching calculations.</returns>
    public async Task<ListSelfAssessmentCalculationsResponse> ListSelfAssessmentCalculationsAsync(ListSelfAssessmentCalculationsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<ListSelfAssessmentCalculationsResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve all the tax calculation data for a given National Insurance number and Calculation ID.
    /// </summary>
    /// <param name="request">A <see cref="RetrieveSelfAssessmentCalculationRequest"/> identifying the calculation to retrieve.</param>
    /// <returns>A <see cref="RetrieveSelfAssessmentCalculationResponse"/> containing the detailed tax calculation data.</returns>
    public RetrieveSelfAssessmentCalculationResponse RetrieveSelfAssessmentCalculation(RetrieveSelfAssessmentCalculationRequest request) {
      return this.ExecuteRequest<RetrieveSelfAssessmentCalculationResponse>(request);
    }

    /// <summary>
    /// Retrieve all the tax calculation data for a given National Insurance number and Calculation ID asynchronously.
    /// </summary>
    /// <param name="request">A <see cref="RetrieveSelfAssessmentCalculationRequest"/> identifying the calculation to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="RetrieveSelfAssessmentCalculationResponse"/> with the detailed tax calculation data.</returns>
    public async Task<RetrieveSelfAssessmentCalculationResponse> RetrieveSelfAssessmentCalculationAsync(RetrieveSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<RetrieveSelfAssessmentCalculationResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Submit a final declaration for a tax year by agreeing to the HMRC's tax calculation.
    /// </summary>
    /// <param name="request">A <see cref="SubmitFinalAssessmentRequest"/> containing the data required to submit the final assessment.</param>
    /// <returns>A <see cref="SubmitFinalAssessmentResponse"/> describing the result of the submission.</returns>
    public SubmitFinalAssessmentResponse SubmitFinalAssessment(SubmitFinalAssessmentRequest request) {
      return this.ExecuteRequest<SubmitFinalAssessmentResponse>(request);
    }

    /// <summary>
    /// Submit a final declaration for a tax year by agreeing to the HMRC's tax calculation asynchronously.
    /// </summary>
    /// <param name="request">A <see cref="SubmitFinalAssessmentRequest"/> containing the data required to submit the final assessment.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SubmitFinalAssessmentResponse"/> describing the result of the submission.</returns>
    public async Task<SubmitFinalAssessmentResponse> SubmitFinalAssessmentAsync(SubmitFinalAssessmentRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<SubmitFinalAssessmentResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Triggers a self assessment tax calculation for a given tax year.
    /// </summary>
    /// <param name="request">A <see cref="TriggerSelfAssessmentCalculationRequest"/> identifying the tax year and customer for which to trigger the calculation.</param>
    /// <returns>A <see cref="TriggerSelfAssessmentCalculationResponse"/> containing information about the triggered calculation request.</returns>
    public TriggerSelfAssessmentCalculationResponse TriggerCalculation(TriggerSelfAssessmentCalculationRequest request) {
      return this.ExecuteRequest<TriggerSelfAssessmentCalculationResponse>(request);
    }

    /// <summary>
    /// Triggers a self assessment tax calculation for a given tax year asynchronously.
    /// </summary>
    /// <param name="request">A <see cref="TriggerSelfAssessmentCalculationRequest"/> identifying the tax year and customer for which to trigger the calculation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TriggerSelfAssessmentCalculationResponse"/> with information about the triggered calculation request.</returns>
    public async Task<TriggerSelfAssessmentCalculationResponse> TriggerCalculationAsync(TriggerSelfAssessmentCalculationRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<TriggerSelfAssessmentCalculationResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
