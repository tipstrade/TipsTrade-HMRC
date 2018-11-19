using RestSharp;
using System;
using System.Reflection;
using TipsTrade.HMRC.Api.Attributes;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;

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
    /// <summary>Creates a test agent user with the specified services.</summary>
    /// <param name="request">The requested service names.</param>
    public AgentResult CreateUser(CreateAgentRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<AgentResult>(restRequest);
    }

    /// <summary>Creates a test individual user with the specified services.</summary>
    /// <param name="request">The requested service names.</param>
    public IndividualResult CreateUser(CreateIndividualRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<IndividualResult>(restRequest);
    }

    /// <summary>Creates a test organisation user with the specified services.</summary>
    /// <param name="request">The requested service names.</param>
    public OrganisationResult CreateUser(CreateOrganisationRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<OrganisationResult>(restRequest);
    }
    #endregion
  }
}
