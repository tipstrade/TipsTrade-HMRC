using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd {
  /// <summary>The API that exposes Self Employment Business (MTD) function.</summary>
  public class SelfEmploymentBusinessMtdApi : IApi, IClient {
    #region Properties
    Client IClient.Client { get; set; }

    /// <inheritdoc>
    public string Description => "Create or amend a self-employment annual summary for a tax year.";

    /// <inheritdoc>
    public bool IsStable => true;

    /// <inheritdoc>
    public string Location => "individuals/business/self-employment";

    /// <inheritdoc>
    public string Name => "Self Employment Business (MTD) API";

    /// <inheritdoc>
    public string Version => "5.0";
    #endregion

    #region Methods
    /// <summary>Submit the cumulative period income and expenses for a self-employment business that occurred between two dates.</summary>
    public AmendCumulativePeriodSummaryResponse CreateOrAmendCumulativePeriodSummary(AmendCumulativePeriodSummaryRequest request) {
      var restrequest = this.CreateRequest(request);

      return this.ExecuteRequest<AmendCumulativePeriodSummaryResponse>(restrequest);
    }

    /// <summary>Retrieve the cumulative period income and expenses for a self-employment business that occurred between two dates.</summary>
    public GetCumulativePeriodSummaryResponse GetCumulativePeriodSummary(GetCumulativePeriodSummaryRequest request) {
      var restrequest = this.CreateRequest(request);

      return this.ExecuteRequest<GetCumulativePeriodSummaryResponse>(restrequest);
    }
    #endregion
  }
}
