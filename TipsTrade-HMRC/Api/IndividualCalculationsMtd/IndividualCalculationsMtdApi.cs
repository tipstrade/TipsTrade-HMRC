using TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd {
  /// <summary>The API that exposes Individual Calculations (MTD) function.</summary>
  public class IndividualCalculationsMtdApi : IApi, IClient {
    #region Properties
    Client IClient.Client { get; set; }

    /// <inheritdoc>
    public string Description => "Trigger, list, retrieve and submit a customer’s self-assessment tax calculation.";

    /// <inheritdoc>
    public bool IsStable => false;

    /// <inheritdoc>
    public string Location => "individuals/calculations";

    /// <inheritdoc>
    public string Name => "Individual Calculations (MTD) API";

    /// <inheritdoc>
    public string Version => "8.0";
    #endregion

    #region Methods
    /// <summary>List Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
    public ListSelfAssessmentCalculationsResponse ListSelfAssessmentCalculations(ListSelfAssessmentCalculationsRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<ListSelfAssessmentCalculationsResponse>(restRequest);
    }

    /// <summary>Retrieve all the tax calculation data for a given National Insurance number and Calculation ID.</summary>
    public RetrieveSelfAssessmentCalculationResponse RetrieveSelfAssessmentCalculation(RetrieveSelfAssessmentCalculationRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<RetrieveSelfAssessmentCalculationResponse>(restRequest);
    }

    /// <summary>Triggers a self assessment tax calculation for a given tax year.</summary>
    public TriggerSelfAssessmentCalculationResponse TriggerCalculation(TriggerSelfAssessmentCalculationRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<TriggerSelfAssessmentCalculationResponse>(restRequest);
    }
    #endregion
  }
}
