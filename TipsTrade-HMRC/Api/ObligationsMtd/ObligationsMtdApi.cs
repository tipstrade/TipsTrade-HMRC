using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;

namespace TipsTrade.HMRC.Api.ObligationsMtd {
  /// <summary>
  /// Provides access to the Obligations (Making Tax Digital) endpoints.
  /// Use this API to retrieve obligations for a user's business income sources and final declarations
  /// (previously known as crystallisations) for a customer's Income Tax account.
  /// Implements <see cref="IApi"/> to describe API metadata and <see cref="IClient"/> to expose the underlying client.
  /// </summary>
  public class ObligationsMtdApi : IApi, IClient {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>
    public string Description => "Retrieve obligations for a user's business income sources.";

    /// <inheritdoc/>
    public bool IsStable => true;

    /// <inheritdoc/>
    public string Location => "obligations/details";

    /// <inheritdoc/>
    public string Name => "Obligations (MTD) API";

    /// <inheritdoc/>
    public string Version => "3.0";
    #endregion

    #region API Methods
    /// <summary>
    /// Retrieve obligations for a user's business income sources.
    /// </summary>
    /// <param name="request">
    /// A <see cref="GetObligationsRequest"/> containing the parameters that identify the user and any filtering
    /// or test scenario options required by the call.
    /// </param>
    /// <returns>
    /// A <see cref="GetObligationsResponse"/> containing the obligations for the specified business income sources.
    /// </returns>
    public GetObligationsResponse GetIncomeAndExpenditureObligations(GetObligationsRequest request) {
      return this.ExecuteRequest<GetObligationsResponse>(request);
    }

    /// <summary>
    /// Asynchronously retrieve obligations for a user's business income sources.
    /// </summary>
    /// <param name="request">
    /// A <see cref="GetObligationsRequest"/> containing the parameters that identify the user and any filtering
    /// or test scenario options required by the call.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="GetObligationsResponse"/>
    /// with the obligations for the specified business income sources.
    /// </returns>
    public async Task<GetObligationsResponse> GetIncomeAndExpenditureObligationsAsync(GetObligationsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<GetObligationsResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve the final declaration (previously known as crystallisation) obligations for a customer's Income Tax account.
    /// </summary>
    /// <param name="request">
    /// A <see cref="GetFinalObligationsRequest"/> that identifies the account and any filtering or test scenario options.
    /// </param>
    /// <returns>
    /// A <see cref="GetFinalObligationsResponse"/> containing the final declaration obligations for the specified account.
    /// </returns>
    public GetFinalObligationsResponse GetFinalObligations(GetFinalObligationsRequest request) {
      return this.ExecuteRequest<GetFinalObligationsResponse>(request);
    }

    /// <summary>
    /// Asynchronously retrieve the final declaration (previously known as crystallisation) obligations for a customer's Income Tax account.
    /// </summary>
    /// <param name="request">
    /// A <see cref="GetFinalObligationsRequest"/> that identifies the account and any filtering or test scenario options.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="GetFinalObligationsResponse"/>
    /// with the final declaration obligations for the specified account.
    /// </returns>
    public async Task<GetFinalObligationsResponse> GetFinalObligationsAsync(GetFinalObligationsRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<GetFinalObligationsResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
