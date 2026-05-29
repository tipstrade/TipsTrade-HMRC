using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd {
  /// <summary>Service that exposes Business Details (MTD) functions, supporting dependency injection.</summary>
  public class BusinessDetailsMtdService : HmrcServiceBase, IRequiresAntiFraud {
    /// <inheritdoc/>
    public override string Description => "Business Details API to retrieve detailed information about a customer's self-employment or property business.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "individuals/business/details";

    /// <inheritdoc/>
    public override string Name => "Business Details (MTD) API";

    /// <inheritdoc/>
    public override string Version => "2.0";

    /// <summary>Initialises a new instance using dependency-injected options.</summary>
    public BusinessDetailsMtdService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory) : base(options, httpClientFactory) { }

    /// <summary>Initialises a new instance using a plain <see cref="HmrcOptions"/> object.</summary>
    public BusinessDetailsMtdService(HmrcOptions options, IHttpClientFactory httpClientFactory) : base(options, httpClientFactory) { }

    /// <summary>Create or amend the type of quarterly reporting period used for a business for a specific tax year.</summary>
    public AmendQuarterlyPeriodTypeResponse CreateOrAmendQuarterlyPeriodType(AmendQuarterlyPeriodTypeRequest request) {
      return this.ExecuteRequest<AmendQuarterlyPeriodTypeResponse>(request);
    }

    /// <summary>Asynchronously create or amend the type of quarterly reporting period used for a business for a specific tax year.</summary>
    public async Task<AmendQuarterlyPeriodTypeResponse> CreateOrAmendQuarterlyPeriodTypeAsync(AmendQuarterlyPeriodTypeRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<AmendQuarterlyPeriodTypeResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Gets additional information for a specific business income source.</summary>
    public GetBusinessDetailsResponse GetBusinessDetails(GetBusinessDetailsRequest request) {
      return this.ExecuteRequest<GetBusinessDetailsResponse>(request);
    }

    /// <summary>Asynchronously gets additional information for a specific business income source.</summary>
    public async Task<GetBusinessDetailsResponse> GetBusinessDetailsAsync(GetBusinessDetailsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<GetBusinessDetailsResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Gets all details of a user's business income sources.</summary>
    public ListBusinessDetailsResponse ListBusinessDetails(ListBusinessDetailsRequest request) {
      return this.ExecuteRequest<ListBusinessDetailsResponse>(request);
    }

    /// <summary>Asynchronously gets all details of a user's business income sources.</summary>
    public async Task<ListBusinessDetailsResponse> ListBusinessDetailsAsync(ListBusinessDetailsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<ListBusinessDetailsResponse>(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
