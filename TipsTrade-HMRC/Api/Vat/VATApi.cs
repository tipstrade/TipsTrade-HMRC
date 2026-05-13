using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>The API that exposes VAT functions.</summary>
  public class VatApi : IApi, IClient, IRequiresAntiFraud {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>
    public string Description => "An API for providing VAT data.";

    /// <inheritdoc/>
    public bool IsStable => false;

    /// <inheritdoc/>
    public string Location => "organisations/vat";

    /// <inheritdoc/>
    public string Name => "VAT (MTD) API";

    /// <inheritdoc/>
    public string Version => "1.0";
    #endregion

    #region API Methods
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

    /// <summary>Submit VAT return for period.</summary>
    /// <param name="request">The submission request.</param>
    public SubmitResponse SubmitReturn(SubmitRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<SubmitResponse>(restRequest);
    }

    /// <summary>Retrieve VAT liabilities asynchronously.</summary>
    public async Task<LiabilitiesResponse> GetLiabilitiesAsync(LiabilitiesRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);

      return await this.ExecuteRequestAsync<LiabilitiesResponse>(
        restRequest,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve VAT obligations asynchronously.</summary>
    public async Task<ObligationResponse> GetObligationsAsync(ObligationsRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);

      return await this.ExecuteRequestAsync<ObligationResponse>(
        restRequest,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve VAT payments asynchronously.</summary>
    public async Task<PaymentsResponse> GetPaymentsAsync(PaymentsRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);

      return await this.ExecuteRequestAsync<PaymentsResponse>(
        restRequest,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve a submitted VAT return asynchronously.</summary>
    public async Task<ReturnResponse> GetReturnAsync(ReturnRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);

      return await this.ExecuteRequestAsync<ReturnResponse>(
        restRequest,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Submit VAT return for period asynchronously.</summary>
    public async Task<SubmitResponse> SubmitReturnAsync(SubmitRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);

      return await this.ExecuteRequestAsync<SubmitResponse>(
        restRequest,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
