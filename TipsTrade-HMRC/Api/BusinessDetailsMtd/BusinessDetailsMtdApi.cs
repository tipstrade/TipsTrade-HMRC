using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd {
  /// <summary>
  /// The API that exposes Business Details (MTD) functions.
  /// Provides methods to retrieve and amend details for a customer's self-employment or property business income sources.
  /// </summary>
  public class BusinessDetailsMtdApi : IApi, IClient {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>
    public string Description => "Business Details API to retrieve detailed information about a customer’s self-employment or property business.";

    /// <inheritdoc/>
    public bool IsStable => true;

    /// <inheritdoc/>
    public string Location => "individuals/business/details";

    /// <inheritdoc/>
    public string Name => "Business Details (MTD) API";

    /// <inheritdoc/>
    public string Version => "2.0";
    #endregion

    #region Methods
    /// <summary>
    /// Create or amend the type of quarterly reporting period used for a business for a specific tax year.
    /// </summary>
    /// <param name="request">
    /// The <see cref="AmendQuarterlyPeriodTypeRequest"/> describing the business identifier and the quarterly period type
    /// to create or amend.
    /// </param>
    /// <returns>
    /// An <see cref="AmendQuarterlyPeriodTypeResponse"/> containing the result of the create or amend operation.
    /// </returns>
    public AmendQuarterlyPeriodTypeResponse CreateOrAmendQuarterlyPeriodType(AmendQuarterlyPeriodTypeRequest request) {

      return this.ExecuteRequest<AmendQuarterlyPeriodTypeResponse>(request);
    }

    /// <summary>
    /// Asynchronously create or amend the type of quarterly reporting period used for a business for a specific tax year.
    /// </summary>
    /// <param name="request">
    /// The <see cref="AmendQuarterlyPeriodTypeRequest"/> describing the business identifier and the quarterly period type
    /// to create or amend.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="AmendQuarterlyPeriodTypeResponse"/>
    /// with the result of the create or amend operation.
    /// </returns>
    public async Task<AmendQuarterlyPeriodTypeResponse> CreateOrAmendQuarterlyPeriodTypeAsync(AmendQuarterlyPeriodTypeRequest request, CancellationToken cancellationToken = default) {

      return await this.ExecuteRequestAsync<AmendQuarterlyPeriodTypeResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets additional information for a specific business income source.
    /// </summary>
    /// <param name="request">
    /// The <see cref="GetBusinessDetailsRequest"/> that identifies the business income source to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="GetBusinessDetailsResponse"/> containing detailed information for the specified business income source.
    /// </returns>
    public GetBusinessDetailsResponse GetBusinessDetails(GetBusinessDetailsRequest request) {
      return this.ExecuteRequest<GetBusinessDetailsResponse>(request);
    }

    /// <summary>
    /// Asynchronously gets additional information for a specific business income source.
    /// </summary>
    /// <param name="request">
    /// The <see cref="GetBusinessDetailsRequest"/> that identifies the business income source to retrieve.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="GetBusinessDetailsResponse"/>
    /// with detailed information for the specified business income source.
    /// </returns>
    public async Task<GetBusinessDetailsResponse> GetBusinessDetailsAsync(GetBusinessDetailsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<GetBusinessDetailsResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all details of a user's business income sources.
    /// </summary>
    /// <param name="request">
    /// The <see cref="ListBusinessDetailsRequest"/> that may include test scenario or filtering information as required.
    /// </param>
    /// <returns>
    /// A <see cref="ListBusinessDetailsResponse"/> containing summaries and details for all business income sources for the user.
    /// </returns>
    public ListBusinessDetailsResponse ListBusinessDetails(ListBusinessDetailsRequest request) {
      return this.ExecuteRequest<ListBusinessDetailsResponse>(request);
    }

    /// <summary>
    /// Asynchronously gets all details of a user's business income sources.
    /// </summary>
    /// <param name="request">
    /// The <see cref="ListBusinessDetailsRequest"/> that may include test scenario or filtering information as required.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="ListBusinessDetailsResponse"/>
    /// with summaries and details for all business income sources for the user.
    /// </returns>
    public async Task<ListBusinessDetailsResponse> ListBusinessDetailsAsync(ListBusinessDetailsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<ListBusinessDetailsResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
