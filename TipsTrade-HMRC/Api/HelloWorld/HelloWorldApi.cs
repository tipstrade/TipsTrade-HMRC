using TipsTrade.HMRC.Api.HelloWorld.Model;
using TipsTrade.HMRC.HelloWorld.Api.Model;

namespace TipsTrade.HMRC.Api.HelloWorld {
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
      var restRequest = this.CreateRequest(new HelloRequest("application", Authorization.Application));
      return this.ExecuteRequest<MessageResponse>(restRequest).Message;
    }

    /// <summary>Says "Hello User"</summary>
    public string SayHelloUser() {
      var restRequest = this.CreateRequest(new HelloRequest("user", Authorization.User));
      return this.ExecuteRequest<MessageResponse>(restRequest).Message;
    }

    /// <summary>Says "Hello World"</summary>
    public string SayHelloWorld() {
      var restRequest = this.CreateRequest(new HelloRequest("world", Authorization.Open));
      return this.ExecuteRequest<MessageResponse>(restRequest).Message;
    }
    #endregion
  }
}
