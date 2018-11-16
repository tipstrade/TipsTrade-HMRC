using Newtonsoft.Json;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.HelloWorld.Api.Model;

namespace TipsTrade.HMRC.HelloWorld.Api {
  /// <summary>The API that exposes Hello World function.</summary>
  public class HelloWorldApi : IApi, IClient {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <summary>The description of the API.</summary>
    public string Description => "A 'hello world' example of an API on the HMRC API Developer Hub.";

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public bool IsStable => true;

    /// <summary>The relative location of the API.</summary>
    public string Location => "hello";

    /// <summary>The name of the API.</summary>
    public string Name => "Hello World API";

    /// <summary>The version of the API that the client should target.</summary>
    public string Version => "1.0";
    #endregion

    #region Methods
    /// <summary>Says "Hello Application"</summary>
    public string SayHelloApplication() {
      var request = this.CreateRequest("application", authorization: Authorization.Application);
      return this.ExecuteRequest<MessageResponse>(request).Message;
    }

    /// <summary>Says "Hello User"</summary>
    public string SayHelloUser() {
      var request = this.CreateRequest("user", authorization: Authorization.User);
      return this.ExecuteRequest<MessageResponse>(request).Message;
    }

    /// <summary>Says "Hello World"</summary>
    public string SayHelloWorld() {
      var request = this.CreateRequest("world");
      return this.ExecuteRequest<MessageResponse>(request).Message;
    }
    #endregion
  }
}
