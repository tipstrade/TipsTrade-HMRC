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
    /// <summary>Creates a test user  with the specified services.</summary>
    /// <param name="request">The requested service names.</param>
    /// <typeparam name="T">The type of user to create.</typeparam>
    public T CreateUser<T>(ICreateTestUserRequest request) where T : UserResultBase {
      var expectedRequestType = typeof(T).GetCustomAttribute<RequestTypeAttribute>().RequestType;
      if (request.GetType() != expectedRequestType) {
        throw new InvalidOperationException($"{typeof(T)} expects a request type of {expectedRequestType}.");
      }

      var endpoint = expectedRequestType.GetCustomAttribute<EndpointAttribute>().Endpoint;

      var restRequest = this.CreateRequest(endpoint, Method.POST, Authorization.Application);
      restRequest.AddJsonBodyNewtonsoft(request);

      return this.ExecuteRequest<T>(restRequest);
    }
    #endregion
  }
}
