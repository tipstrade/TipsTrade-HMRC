using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd {
  /// <summary>
  /// Provides access to the Self Employment Business (MTD) endpoints.
  /// Use this API to create, amend and retrieve cumulative period summaries for a self-employment business.
  /// This class implements <see cref="IApi"/> to describe the API metadata and <see cref="IClient"/> to expose the underlying client.
  /// </summary>
  public class SelfEmploymentBusinessMtdApi : IApi, IClient {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>
    public string Description => "Create or amend a self-employment annual summary for a tax year.";

    /// <inheritdoc/>
    public bool IsStable => true;

    /// <inheritdoc/>
    public string Location => "individuals/business/self-employment";

    /// <inheritdoc/>
    public string Name => "Self Employment Business (MTD) API";

    /// <inheritdoc/>
    public string Version => "5.0";
    #endregion

    #region Methods
    /// <summary>
    /// Submit or amend the cumulative period income and expenses for a self-employment business that occurred between two dates.
    /// This method will create a new cumulative period summary or amend an existing one for the specified business and period.
    /// </summary>
    /// <param name="request">
    /// An <see cref="AmendCumulativePeriodSummaryRequest"/> describing the target business, the period and the summary payload to submit.
    /// </param>
    /// <returns>
    /// An <see cref="AmendCumulativePeriodSummaryResponse"/> containing the result of the create or amend operation.
    /// On success the response will indicate the operation status and any server-provided identifiers.
    /// </returns>
    public AmendCumulativePeriodSummaryResponse CreateOrAmendCumulativePeriodSummary(AmendCumulativePeriodSummaryRequest request) {
      return this.ExecuteRequest<AmendCumulativePeriodSummaryResponse>(request);
    }

    /// <summary>
    /// Asynchronously submit or amend the cumulative period income and expenses for a self-employment business
    /// that occurred between two dates. This method will create a new cumulative period summary or amend an
    /// existing one for the specified business and period.
    /// </summary>
    /// <param name="request">
    /// An <see cref="AmendCumulativePeriodSummaryRequest"/> describing the target business, the period and the
    /// summary payload to submit.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation. If omitted the
    /// operation will run until completion or failure.
    /// </param>
    /// <returns>
    /// A <see cref="Task{AmendCumulativePeriodSummaryResponse}"/> that represents the asynchronous operation.
    /// The task result is an <see cref="AmendCumulativePeriodSummaryResponse"/> containing the result of the
    /// create or amend operation. On success the response will indicate the operation status and any
    /// server-provided identifiers.
    /// </returns>
    /// <remarks>
    /// This method internally forwards the request to <c>ExecuteRequestAsync{AmendCumulativePeriodSummaryResponse}</c>
    /// and uses <c>ConfigureAwait(false)</c> for the awaited task.
    /// </remarks>
    public async Task<AmendCumulativePeriodSummaryResponse> CreateOrAmendCumulativePeriodSummaryAsync(AmendCumulativePeriodSummaryRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<AmendCumulativePeriodSummaryResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve the cumulative period income and expenses for a self-employment business that occurred between two dates.
    /// Use this method to fetch the previously submitted cumulative summary for the specified business and period.
    /// </summary>
    /// <param name="request">
    /// A <see cref="GetCumulativePeriodSummaryRequest"/> that identifies the business and the period to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="GetCumulativePeriodSummaryResponse"/> containing the requested cumulative period summary data.
    /// If no summary exists for the specified period the response will reflect that condition according to the underlying API behaviour.
    /// </returns>
    public GetCumulativePeriodSummaryResponse GetCumulativePeriodSummary(GetCumulativePeriodSummaryRequest request) {
      return this.ExecuteRequest<GetCumulativePeriodSummaryResponse>(request);
    }

    /// <summary>
    /// Asynchronously retrieve the cumulative period income and expenses for a self-employment business
    /// that occurred between two dates.
    /// </summary>
    /// <param name="request">
    /// A <see cref="GetCumulativePeriodSummaryRequest"/> that identifies the business and the period to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation. If omitted the
    /// operation will run until completion or failure.
    /// </param>
    /// <returns>
    /// A <see cref="Task{GetCumulativePeriodSummaryResponse}"/> that represents the asynchronous operation.
    /// The task result is a <see cref="GetCumulativePeriodSummaryResponse"/> containing the requested cumulative
    /// period summary data. If no summary exists for the specified period the response will reflect that condition
    /// according to the underlying API behaviour.
    /// </returns>
    /// <remarks>
    /// This method forwards the request to <c>ExecuteRequestAsync{GetCumulativePeriodSummaryResponse}</c>
    /// and uses <c>ConfigureAwait(false)</c> for the awaited task to avoid capturing the calling context.
    /// </remarks>
    public async Task<GetCumulativePeriodSummaryResponse> GetCumulativePeriodSummaryAsync(GetCumulativePeriodSummaryRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<GetCumulativePeriodSummaryResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
