using RestSharp;
using TipsTrade.HMRC.Api.CreateTestUser.Model;

namespace TipsTrade.HMRC.Api.CreateTestUser {
  /// <summary>The API that exposes Create Test User functions.</summary>
  public class CreateTestUserApi : IApi, IClient {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <summary>The description of the API.</summary>
    public string Description => "An API to create test users for testing in our sandbox with user-restricted endpoints.";

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public bool IsStable => false;

    /// <summary>The relative location of the API.</summary>
    public string Location => "create-test-user";

    /// <summary>The name of the API.</summary>
    public string Name => "Create Test User API";

    /// <summary>The version of the API that the client should target.</summary>
    public string Version => "1.0";
    #endregion

    #region Methods
    /// <summary>Creates a test organisation user with the specified services.</summary>
    /// <param name="request">The services to request.</param>
    public OrganisationResult CreateOrganisation(CreateOrganisationRequest request) {
      var restRequest = this.CreateRequest("organisations", Method.POST, Authorization.Application);
      restRequest.AddHeader("Content-Type", "application/json");
      restRequest.AddJsonBody(request);
      
      // TODO: Fix a nigh-on empty user being returned
      return this.ExecuteRequest<OrganisationResult>(restRequest);
    }
    #endregion
  }
}
