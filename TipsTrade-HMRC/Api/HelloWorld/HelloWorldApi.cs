using System.Threading;
using System.Threading.Tasks;
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
      return this.ExecuteRequest<MessageResponse>(
        new HelloRequest("application", Authorization.Application)
        ).Message;
    }

    /// <summary>Says "Hello Application"</summary>
    public async Task<string> SayHelloApplicationAsync(CancellationToken cancellationToken = default) {
      var response = await this.ExecuteRequestAsync<MessageResponse>(
        new HelloRequest("application", Authorization.Application),
        cancellationToken).ConfigureAwait(false);

      return response.Message;
    }

    /// <summary>Says "Hello User"</summary>
    public string SayHelloUser() {
      return this.ExecuteRequest<MessageResponse>(
        new HelloRequest("user", Authorization.User)
        ).Message;
    }

    /// <summary>Says "Hello User"</summary>
    public async Task<string> SayHelloUserAsync(CancellationToken cancellationToken = default) {
      var response = await this.ExecuteRequestAsync<MessageResponse>(
        new HelloRequest("user", Authorization.User),
        cancellationToken
        ).ConfigureAwait(false);

      return response.Message;
    }

    /// <summary>Says "Hello World"</summary>
    public string SayHelloWorld() {
      return this.ExecuteRequest<MessageResponse>(
        new HelloRequest("world", Authorization.Open)
        ).Message;
    }

    /// <summary>Says "Hello World"</summary>
    public async Task<string> SayHelloWorldAsync(CancellationToken cancellationToken = default) {
      var response = await this.ExecuteRequestAsync<MessageResponse>(
        new HelloRequest("world", Authorization.Open),
        cancellationToken
      ).ConfigureAwait(false);

      return response.Message;
    }
    #endregion
  }
}
