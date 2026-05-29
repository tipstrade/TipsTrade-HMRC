using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd {
  /// <summary>Service that exposes Self Assessment Test Support (MTD) functions, supporting dependency injection.</summary>
  public class SelfAssessmentTestSupportMtdService : HmrcServiceBase {
    /// <inheritdoc/>
    public override string Description => "Self Assessment Test API for modifying stateful test data.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "individuals/self-assessment-test-support";

    /// <inheritdoc/>
    public override string Name => "Self Assessment Test Support (MTD) API";

    /// <inheritdoc/>
    public override string Version => "1.0";

    /// <summary>Initialises a new instance using dependency-injected options.</summary>
    public SelfAssessmentTestSupportMtdService(IOptions<HmrcOptions> options) : base(options) { }

    /// <summary>Initialises a new instance using a plain <see cref="HmrcOptions"/> object.</summary>
    public SelfAssessmentTestSupportMtdService(HmrcOptions options) : base(options) { }

    /// <summary>Delete stateful test data, optionally scoped to a National Insurance number.</summary>
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(string niNumber = null) {
      return DeleteStatefulTestData(new DeleteStatefulTestDataRequest { NiNumber = niNumber });
    }

    /// <summary>Delete stateful test data using a request object.</summary>
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(DeleteStatefulTestDataRequest request) {
      return this.ExecuteRequest<DeleteStatefulTestDataResponse>(request);
    }

    /// <summary>Asynchronously delete stateful test data, optionally scoped to a National Insurance number.</summary>
    public async Task<DeleteStatefulTestDataResponse> DeleteStatefulTestDataAsync(string niNumber = null, CancellationToken cancellationToken = default) {
      return await DeleteStatefulTestDataAsync(new DeleteStatefulTestDataRequest { NiNumber = niNumber }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously delete stateful test data using a request object.</summary>
    public async Task<DeleteStatefulTestDataResponse> DeleteStatefulTestDataAsync(DeleteStatefulTestDataRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<DeleteStatefulTestDataResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Create a test business income source for use within the sandbox environment.</summary>
    public CreateTestBusinessResponse CreateBusinessIncomeSource(CreateTestBusinessRequest request) {
      return this.ExecuteRequest<CreateTestBusinessResponse>(request);
    }

    /// <summary>Asynchronously create a test business income source for use within the sandbox environment.</summary>
    public async Task<CreateTestBusinessResponse> CreateBusinessIncomeSourceAsync(CreateTestBusinessRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<CreateTestBusinessResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Create or amend a test ITSA status for a specified customer.</summary>
    public CreateTestItsaStatusResponse CreateTestItsaStatus(CreateTestItsaStatusRequest request) {
      return this.ExecuteRequest<CreateTestItsaStatusResponse>(request);
    }

    /// <summary>Asynchronously create or amend a test ITSA status for a specified customer.</summary>
    public async Task<CreateTestItsaStatusResponse> CreateTestItsaStatusAsync(CreateTestItsaStatusRequest request, CancellationToken cancellationToken = default) {
      return await this.ExecuteRequestAsync<CreateTestItsaStatusResponse>(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
