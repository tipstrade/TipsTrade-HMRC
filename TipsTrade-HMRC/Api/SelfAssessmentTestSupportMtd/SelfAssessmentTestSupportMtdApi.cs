using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd {
  /// <summary>The API that Self Assessment Test Support (MTD) function.</summary>
  public class SelfAssessmentTestSupportMtdApi : IApi, IClient {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <inheritdoc>
    public string Description => "\"Self Assessment Test API for modifying stateful test data.";

    /// <inheritdoc>
    public bool IsStable => true;

    /// <inheritdoc>
    public string Location => "individuals/self-assessment-test-support";

    /// <inheritdoc>
    public string Name => "Self Assessment Test Support (MTD) API";

    /// <inheritdoc>
    public string Version => "1.0";
    #endregion

    #region Main Methods
    /// <summary>
    /// Allows a developer to delete stateful test data supplied by them in the sandbox environment.
    /// </summary>
    /// <param name="niNumber">If supplied, the endpoint deletes only stateful test data stored against the vendor that is associated with the specified National Insurance number.</param>
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(string niNumber = null) {
      return DeleteStatefulTestData(new DeleteStatefulTestDataRequest {
        NiNumber = niNumber
      });
    }

    /// <summary>
    /// Allows a developer to delete stateful test data supplied by them in the sandbox environment.
    /// </summary>
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(DeleteStatefulTestDataRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<DeleteStatefulTestDataResponse>(restRequest);
    }
    #endregion

    #region Checkpoint methods
    #endregion

    #region Business Income Source methods
    /// <summary>Create a test business for use within the Sandbox environment.</summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public CreateTestBusinessResponse CreateBusinessIncomeSource(CreateTestBusinessRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<CreateTestBusinessResponse>(restRequest);
    }
    #endregion

    #region ITSA status methods
    #endregion
  }
}
