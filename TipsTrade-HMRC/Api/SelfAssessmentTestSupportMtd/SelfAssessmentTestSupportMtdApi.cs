using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd {
  /// <summary>The API that Self Assessment Test Support (MTD) function.</summary>
  public class SelfAssessmentTestSupportMtdApi : IApi, IClient {
    #region Properties
    /// <inheritdoc/>
    public Client Client { get; set; }

    /// <inheritdoc/>

    public string Description => "Self Assessment Test API for modifying stateful test data.";

    /// <inheritdoc/>
    public bool IsStable => true;

    /// <inheritdoc/>
    public string Location => "individuals/self-assessment-test-support";

    /// <inheritdoc/>
    public string Name => "Self Assessment Test Support (MTD) API";

    /// <inheritdoc/>
    public string Version => "1.0";
    #endregion

    #region Main API Methods
    /// <summary>
    /// Allows a developer to delete stateful test data supplied by them in the sandbox environment.
    /// If <paramref name="niNumber"/> is provided only stateful test data associated with that National
    /// Insurance number and the current vendor is deleted; otherwise all stateful test data supplied by
    /// the vendor is removed.
    /// </summary>
    /// <param name="niNumber">
    /// Optional. The National Insurance number used to scope the deletion to a specific customer's test data.
    /// If <c>null</c> or omitted the endpoint deletes all stateful test data supplied by the vendor.
    /// </param>
    /// <returns>
    /// A <see cref="DeleteStatefulTestDataResponse"/> describing the outcome of the delete operation.
    /// The response will indicate success or provide error details when the operation fails.
    /// </returns>
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(string niNumber = null) {
      return DeleteStatefulTestData(new DeleteStatefulTestDataRequest {
        NiNumber = niNumber
      });
    }

    /// <summary>
    /// Allows a developer to delete stateful test data supplied by them in the sandbox environment.
    /// </summary>
    /// <param name="request">
    /// A <see cref="DeleteStatefulTestDataRequest"/> that contains the parameters used to identify which
    /// stateful test data should be deleted (for example a National Insurance number to scope the deletion).
    /// </param>
    /// <returns>
    /// A <see cref="DeleteStatefulTestDataResponse"/> describing the outcome of the delete operation.
    /// </returns>
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(DeleteStatefulTestDataRequest request) {
      return this.ExecuteRequest<DeleteStatefulTestDataResponse>(request);
    }

    /// <summary>
    /// Asynchronously allows a developer to delete stateful test data supplied by them in the sandbox environment.
    /// If <paramref name="niNumber"/> is provided only stateful test data associated with that National Insurance
    /// number and the current vendor is deleted; otherwise all stateful test data supplied by the vendor is removed.
    /// </summary>
    /// <param name="niNumber">
    /// Optional. The National Insurance number used to scope the deletion to a specific customer's test data.
    /// If <c>null</c> or omitted the endpoint deletes all stateful test data supplied by the vendor.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous delete operation. The task result is a
    /// <see cref="DeleteStatefulTestDataResponse"/> describing the outcome.
    /// </returns>
    public async Task<DeleteStatefulTestDataResponse> DeleteStatefulTestDataAsync(string niNumber = null, CancellationToken cancellationToken = default) {
      return await DeleteStatefulTestDataAsync(new DeleteStatefulTestDataRequest {
        NiNumber = niNumber
      }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously allows a developer to delete stateful test data supplied by them in the sandbox environment.
    /// </summary>
    /// <param name="request">
    /// A <see cref="DeleteStatefulTestDataRequest"/> that contains the parameters used to identify which
    /// stateful test data should be deleted.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous delete operation. The task result is a
    /// <see cref="DeleteStatefulTestDataResponse"/> describing the outcome.
    /// </returns>
    public async Task<DeleteStatefulTestDataResponse> DeleteStatefulTestDataAsync(DeleteStatefulTestDataRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<DeleteStatefulTestDataResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion

    #region Checkpoint API methods
    #endregion

    #region Business Income Source API methods
    /// <summary>
    /// Create a test business income source for use within the sandbox environment.
    /// This method issues a request to create a business that can be used in sandbox test scenarios.
    /// </summary>
    /// <param name="request">
    /// A <see cref="CreateTestBusinessRequest"/> containing the details required to create the test business.
    /// The request may include fields that control the business type, identifiers and any scenario-specific options.
    /// </param>
    /// <returns>
    /// A <see cref="CreateTestBusinessResponse"/> containing details of the created test business or information
    /// about why creation failed.
    /// </returns>
    public CreateTestBusinessResponse CreateBusinessIncomeSource(CreateTestBusinessRequest request) {
      return this.ExecuteRequest<CreateTestBusinessResponse>(request);
    }

    /// <summary>
    /// Asynchronously creates a test business income source for use within the sandbox environment.
    /// </summary>
    /// <param name="request">
    /// A <see cref="CreateTestBusinessRequest"/> containing the details required to create the test business.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous create operation. The task result is a
    /// <see cref="CreateTestBusinessResponse"/> containing details of the created test business.
    /// </returns>
    public async Task<CreateTestBusinessResponse> CreateBusinessIncomeSourceAsync(CreateTestBusinessRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<CreateTestBusinessResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion

    #region ITSA status API methods
    /// <summary>
    /// Create or amend a test ITSA status for a specified customer for use within the sandbox environment.
    /// This allows test scenarios to simulate different Income Tax Self Assessment statuses for a customer.
    /// </summary>
    /// <param name="request">
    /// A <see cref="CreateTestItsaStatusRequest"/> describing the target customer (for example via NI number)
    /// and the ITSA status data to create or amend in the sandbox.
    /// </param>
    /// <returns>
    /// A <see cref="CreateTestItsaStatusResponse"/> describing the result of the create or amend operation.
    /// </returns>
    public CreateTestItsaStatusResponse CreateTestItsaStatus(CreateTestItsaStatusRequest request) {
      return this.ExecuteRequest<CreateTestItsaStatusResponse>(request);
    }

    /// <summary>
    /// Asynchronously create or amend a test ITSA status for a specified customer for use within the sandbox environment.
    /// </summary>
    /// <param name="request">
    /// A <see cref="CreateTestItsaStatusRequest"/> describing the target customer and the ITSA status data to create or amend.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a
    /// <see cref="CreateTestItsaStatusResponse"/> describing the result of the create or amend action.
    /// </returns>
    public async Task<CreateTestItsaStatusResponse> CreateTestItsaStatusAsync(CreateTestItsaStatusRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<CreateTestItsaStatusResponse>(
        request,
        cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
