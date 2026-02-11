using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd {
  /// <summary>The API that exposes Business Details (MTD) function.</summary>
  public class BusinessDetailsMtdApi : IApi, IClient {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <inheritdoc>
    public string Description => "Business Details API to retrieve detailed information about a customer’s self-employment or property business.";

    /// <inheritdoc>
    public bool IsStable => true;

    /// <inheritdoc>
    public string Location => "individuals/business/details";

    /// <inheritdoc>
    public string Name => "Business Details (MTD) API";

    /// <inheritdoc>
    public string Version => "2.0";
    #endregion

    #region Methods
    /// <summary>Create and amend the type of quarterly reporting period used for a business for a specific tax year.</summary>
    public AmendQuarterlyPeriodTypeResponse CreateOrAmendQuarterlyPeriodType(AmendQuarterlyPeriodTypeRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<AmendQuarterlyPeriodTypeResponse>(restRequest);
    }

    /// <summary>Gets additional information for one of a user's business income source.</summary>
    public GetBusinessDetailsResponse GetBusinessDetails(GetBusinessDetailsRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<GetBusinessDetailsResponse>(restRequest);
    }

    /// <summary>
    /// Gets all details of a user's business income sources.
    /// </summary>
    public ListBusinessDetailsResponse ListBusinessDetails(ListBusinessDetailsRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<ListBusinessDetailsResponse>(restRequest);
    }
    #endregion
  }
}
