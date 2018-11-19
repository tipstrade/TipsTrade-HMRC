using System.Linq;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>The API that exposes VAT functions.</summary>
  public class VatApi : IApi, IClient {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <summary>The description of the API.</summary>
    public string Description => "An API for providing VAT data.";

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public bool IsStable => false;

    /// <summary>The relative location of the API.</summary>
    public string Location => "organisations/vat";

    /// <summary>The name of the API.</summary>
    public string Name => "VAT (MTD) API";

    /// <summary>The version of the API that the client should target.</summary>
    public string Version => "1.0";
    #endregion

    #region Methods
    /// <summary>Retrieve VAT liabilities.</summary>
    /// <param name="request">The date range request.</param>
    public LiabilitiesResponse GetLiabilities(LiabilitiesRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<LiabilitiesResponse>(restRequest);
    }

    /// <summary>Retrieve VAT obligations.</summary>
    /// <param name="request">The obligations request.</param>
    public ObligationResponse GetObligations(ObligationsRequest request) {
      var restRequest = this.CreateRequest(request);

      var resp = this.ExecuteRequest<ObligationResponse>(restRequest);

      // HACK: The Api appears to return all obligations, regardless of status, filter them here
      if (request.Status != null) {
        resp.Value = resp.Value.Except(resp.Value.Where(x => x.Status != request.Status));
      }

      return resp;
    }

    /// <summary>Retrieve VAT payments.</summary>
    /// <param name="request">The date range request.</param>
    public PaymentsResponse GetPayments(PaymentsRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<PaymentsResponse>(restRequest);
    }

    /// <summary>Retrieve a submitted VAT return.</summary>
    /// <param name="request">The retrieval request.</param>
    public ReturnResponse GetReturn(ReturnRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<ReturnResponse>(restRequest);
    }
    #endregion
  }
}
