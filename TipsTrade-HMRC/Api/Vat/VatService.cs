using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.OAuth;
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

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public VatService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Retrieve VAT liabilities.</summary>
    [Obsolete("Use GetLiabilitiesAsync instead. Synchronous methods may cause deadlocks.")]
    public LiabilitiesResponse GetLiabilities(LiabilitiesRequest request) {
      var restRequest = CreateRequest(request);
      return ExecuteRequest<LiabilitiesResponse>(restRequest);
    }

    /// <summary>Retrieve VAT obligations.</summary>
    [Obsolete("Use GetObligationsAsync instead. Synchronous methods may cause deadlocks.")]
    public ObligationResponse GetObligations(ObligationsRequest request) {
      var restRequest = CreateRequest(request);
      return ExecuteRequest<ObligationResponse>(restRequest);
    }

    /// <summary>Retrieve VAT payments.</summary>
    [Obsolete("Use GetPaymentsAsync instead. Synchronous methods may cause deadlocks.")]
    public PaymentsResponse GetPayments(PaymentsRequest request) {
      var restRequest = CreateRequest(request);
      return ExecuteRequest<PaymentsResponse>(restRequest);
    }

    /// <summary>Retrieve a submitted VAT return.</summary>
    [Obsolete("Use GetReturnAsync instead. Synchronous methods may cause deadlocks.")]
    public ReturnResponse GetReturn(ReturnRequest request) {
      var restRequest = CreateRequest(request);
      return ExecuteRequest<ReturnResponse>(restRequest);
    }

    /// <summary>Submit VAT return for period.</summary>
    [Obsolete("Use SubmitReturnAsync instead. Synchronous methods may cause deadlocks.")]
    public SubmitResponse SubmitReturn(SubmitRequest request) {
      var restRequest = CreateRequest(request);
      return ExecuteRequest<SubmitResponse>(restRequest);
    }

    /// <summary>Retrieve VAT liabilities asynchronously.</summary>
    public async Task<LiabilitiesResponse> GetLiabilitiesAsync(LiabilitiesRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<LiabilitiesResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve VAT obligations asynchronously.</summary>
    public async Task<ObligationResponse> GetObligationsAsync(ObligationsRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<ObligationResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve VAT payments asynchronously.</summary>
    public async Task<PaymentsResponse> GetPaymentsAsync(PaymentsRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<PaymentsResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Retrieve a submitted VAT return asynchronously.</summary>
    public async Task<ReturnResponse> GetReturnAsync(ReturnRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<ReturnResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Submit VAT return for period asynchronously.</summary>
    public async Task<SubmitResponse> SubmitReturnAsync(SubmitRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<SubmitResponse>(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
