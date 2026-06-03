using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.OAuth;
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

    /// <summary>Initialises a new instance using dependency-injected services.</summary>
    public SelfAssessmentTestSupportMtdService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService, IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null) : base(options, httpClientFactory, accessTokenProvider, applicationTokenCache, oauthService, tenantProvider, logger) { }

    /// <summary>Delete stateful test data, optionally scoped to a National Insurance number.</summary>
    [Obsolete("Use DeleteStatefulTestDataAsync instead. Synchronous methods may cause deadlocks.")]
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(string? niNumber = null) {
      return DeleteStatefulTestData(new DeleteStatefulTestDataRequest { NiNumber = niNumber });
    }

    /// <summary>Delete stateful test data using a request object.</summary>
    [Obsolete("Use DeleteStatefulTestDataAsync instead. Synchronous methods may cause deadlocks.")]
    public DeleteStatefulTestDataResponse DeleteStatefulTestData(DeleteStatefulTestDataRequest request) {
      return ExecuteRequest<DeleteStatefulTestDataResponse>(request, null);
    }

    /// <summary>Asynchronously delete stateful test data, optionally scoped to a National Insurance number.</summary>
    public async Task<DeleteStatefulTestDataResponse> DeleteStatefulTestDataAsync(string? niNumber = null, CancellationToken cancellationToken = default) {
      return await DeleteStatefulTestDataAsync(new DeleteStatefulTestDataRequest { NiNumber = niNumber }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously delete stateful test data using a request object.</summary>
    public async Task<DeleteStatefulTestDataResponse> DeleteStatefulTestDataAsync(DeleteStatefulTestDataRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<DeleteStatefulTestDataResponse>(request, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Create a test business income source for use within the sandbox environment.</summary>
    [Obsolete("Use CreateBusinessIncomeSourceAsync instead. Synchronous methods may cause deadlocks.")]
    public CreateTestBusinessResponse CreateBusinessIncomeSource(CreateTestBusinessRequest request) {
      return ExecuteRequest<CreateTestBusinessResponse>(request, null);
    }

    /// <summary>Asynchronously create a test business income source for use within the sandbox environment.</summary>
    public async Task<CreateTestBusinessResponse> CreateBusinessIncomeSourceAsync(CreateTestBusinessRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<CreateTestBusinessResponse>(request, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Create or amend a test ITSA status for a specified customer.</summary>
    [Obsolete("Use CreateTestItsaStatusAsync instead. Synchronous methods may cause deadlocks.")]
    public CreateTestItsaStatusResponse CreateTestItsaStatus(CreateTestItsaStatusRequest request) {
      return ExecuteRequest<CreateTestItsaStatusResponse>(request, null);
    }

    /// <summary>Asynchronously create or amend a test ITSA status for a specified customer.</summary>
    public async Task<CreateTestItsaStatusResponse> CreateTestItsaStatusAsync(CreateTestItsaStatusRequest request, CancellationToken cancellationToken = default) {
      return await ExecuteRequestAsync<CreateTestItsaStatusResponse>(request, null, cancellationToken).ConfigureAwait(false);
    }
  }
}
