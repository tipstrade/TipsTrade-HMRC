using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.TestFraudPrevention.Model;

namespace TipsTrade.HMRC.Api.TestFraudPrevention {
  /// <summary>The API that exposes Hello World function.</summary>
  public class TestFraudPreventionApi : IApi, IClient, IRequiresAntiFraud {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>
    public string Description => "An API for testing Fraud Prevention headers.";

    /// <inheritdoc/>
    public bool IsStable => false;

    /// <inheritdoc/>
    public string Location => "test/fraud-prevention-headers";

    /// <inheritdoc/>
    public string Name => "Test Fraud Prevention Headers API";

    /// <inheritdoc/>
    public string Version => "1.0";
    #endregion

    #region API Methods
    /// <summary>
    /// Submits feedback about the fraud prevention headers sent with an API request and returns the API's response.
    /// </summary>
    /// <param name="api">
    /// The identifier of the API endpoint for which feedback is being submitted (see API documentation for allowed values).
    /// </param>
    /// <param name="connectionMethod">
    /// The <see cref="ConnectionMethod"/> describing how the application connected to HMRC for the request.
    /// </param>
    public FeedbackResult GetFeedback(string api, ConnectionMethod connectionMethod) {
      return this.ExecuteRequest<FeedbackResult>(new FeedbackRequest { Api = api, ConnectionMethod = connectionMethod });
    }

    /// <summary>
    /// Asynchronously submits feedback about the fraud prevention headers sent with an API request and returns the API's response.
    /// </summary>
    /// <param name="api">
    /// The identifier of the API endpoint for which feedback is being submitted (see API documentation for allowed values).
    /// </param>
    /// <param name="connectionMethod">
    /// The <see cref="ConnectionMethod"/> describing how the application connected to HMRC for the request.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.
    /// </param>
    public async Task<FeedbackResult> GetFeedbackAsync(string api, ConnectionMethod connectionMethod, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<FeedbackResult>(
        new FeedbackRequest { Api = api, ConnectionMethod = connectionMethod },
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Validates fraud prevention headers submitted with this HTTP request.</summary>
    /// <returns>
    /// A <see cref="ValidateResult"/> containing validation errors and warnings returned by the API.
    /// </returns>
    public ValidateResult Validate() {
      return this.ExecuteRequest<ValidateResult>(new ValidateRequest());
    }

    /// <summary>Validates fraud prevention headers submitted with this HTTP request asynchronously.</summary>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that may be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a <see cref="ValidateResult"/>
    /// containing validation errors and warnings returned by the API.
    /// </returns>
    public async Task<ValidateResult> ValidateAsync(CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<ValidateResult>(
        new ValidateRequest(),
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
