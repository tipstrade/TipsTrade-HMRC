using TipsTrade.HMRC.Api.ObligationsMtd.Model;

namespace TipsTrade.HMRC.Api.ObligationsMtd {
  /// <summary>The API that exposes Obligations (MTD) function.</summary>
  public class ObligationsMtdApi : IApi, IClient {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <inheritdoc>
    public string Description => "Retrieve obligations for a user's business income sources..";

    /// <inheritdoc>
    public bool IsStable => true;

    /// <inheritdoc>
    public string Location => "obligations/details";

    /// <inheritdoc>
    public string Name => "Obligations (MTD) API";

    /// <inheritdoc>
    public string Version => "3.0";
    #endregion

    #region Methods
    /// <summary>Retrieve obligations for a user's business income sources.</summary>
    public GetObligationsResponse GetIncomeAndExpenditureObligations(GetObligationsRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<GetObligationsResponse>(restRequest);
    }

    /// <summary>Retrieve the final declaration (previously known as crystallisation) obligations for a customer’s Income Tax account.</summary>
    public GetFinalObligationsResponse GetFinalObligations(GetFinalObligationsRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<GetFinalObligationsResponse>(restRequest);
    }
    #endregion
  }
}
