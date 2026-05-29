using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>Service that exposes VAT functions, supporting dependency injection.</summary>
  public class VatService : HmrcServiceBase, IRequiresAntiFraud {
    /// <inheritdoc/>
    public override string Description => "An API for providing VAT data.";

    /// <inheritdoc/>
    public override bool IsStable => false;

    /// <inheritdoc/>
    public override string Location => "organisations/vat";

    /// <inheritdoc/>
    public override string Name => "VAT (MTD) API";

    /// <inheritdoc/>
    public override string Version => "1.0";

    /// <summary>Initialises a new instance using dependency-injected options.</summary>
    public VatService(IOptions<HmrcOptions> options) : base(options) { }

    /// <summary>Initialises a new instance using a plain <see cref="HmrcOptions"/> object.</summary>
    public VatService(HmrcOptions options) : base(options) { }

    /// <summary>Retrieve VAT liabilities.</summary>
    public LiabilitiesResponse GetLiabilities(LiabilitiesRequest request) {
      var restRequest = this.CreateRequest(request);
      return this.ExecuteRequest<LiabilitiesResponse>(restRequest);
    }

    /// <summary>Retrieve VAT obligations.</summary>
    public ObligationResponse GetObligations(ObligationsRequest request) {
      var restRequest = this.CreateRequest(request);
      return this.ExecuteRequest<ObligationResponse>(restRequest);
    }

    /// <summary>Retrieve VAT payments.</summary>
    public PaymentsResponse GetPayments(PaymentsRequest request) {
      var restRequest = this.CreateRequest(request);
      return this.ExecuteRequest<PaymentsResponse>(restRequest);
    }

    /// <summary>Retrieve a submitted VAT return.</summary>
    public ReturnResponse GetReturn(ReturnRequest request) {
      var restRequest = this.CreateRequest(request);
      return this.ExecuteRequest<ReturnResponse>(restRequest);
    }

    /// <summary>Submit VAT return for period.</summary>
    public SubmitResponse SubmitReturn(SubmitRequest request) {
      var restRequest = this.CreateRequest(request);
      return this.ExecuteRequest<SubmitResponse>(restRequest);
    }

    /// <summary>Retrieve VAT liabilities asynchronously.</summary>
    public async Task<LiabilitiesResponse> GetLiabilitiesAsync(LiabilitiesRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);
      return await this.ExecuteRequestAsync<LiabilitiesResponse>(restRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve VAT obligations asynchronously.</summary>
    public async Task<ObligationResponse> GetObligationsAsync(ObligationsRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);
      return await this.ExecuteRequestAsync<ObligationResponse>(restRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve VAT payments asynchronously.</summary>
    public async Task<PaymentsResponse> GetPaymentsAsync(PaymentsRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);
      return await this.ExecuteRequestAsync<PaymentsResponse>(restRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve a submitted VAT return asynchronously.</summary>
    public async Task<ReturnResponse> GetReturnAsync(ReturnRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);
      return await this.ExecuteRequestAsync<ReturnResponse>(restRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Submit VAT return for period asynchronously.</summary>
    public async Task<SubmitResponse> SubmitReturnAsync(SubmitRequest request, CancellationToken cancellationToken = default) {
      var restRequest = this.CreateRequest(request);
      return await this.ExecuteRequestAsync<SubmitResponse>(restRequest, cancellationToken).ConfigureAwait(false);
    }
  }
}
