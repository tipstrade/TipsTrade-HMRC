using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.HelloWorld.Model;
using TipsTrade.HMRC.HelloWorld.Api.Model;

namespace TipsTrade.HMRC.Api.HelloWorld {
  /// <summary>Service that exposes Hello World functions, supporting dependency injection.</summary>
  public class HelloWorldService : HmrcServiceBase {
    /// <inheritdoc/>
    public override string Description => "A 'hello world' example of an API on the HMRC API Developer Hub.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "hello";

    /// <inheritdoc/>
    public override string Name => "Hello World API";

    /// <inheritdoc/>
    public override string Version => "1.0";

    /// <summary>Initialises a new instance using dependency-injected options.</summary>
    public HelloWorldService(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory, ApplicationTokenCache applicationTokenCache) : base(options, httpClientFactory, applicationTokenCache) { }

    /// <summary>Initialises a new instance using a plain <see cref="HmrcOptions"/> object.</summary>
    public HelloWorldService(HmrcOptions options, IHttpClientFactory httpClientFactory, ApplicationTokenCache applicationTokenCache) : base(options, httpClientFactory, applicationTokenCache) { }

    /// <summary>Says "Hello Application"</summary>
    public string SayHelloApplication() {
      return this.ExecuteRequest<MessageResponse>(
        new HelloRequest("application", Authorization.Application)
        ).Message;
    }

    /// <summary>Says "Hello Application" asynchronously.</summary>
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

    /// <summary>Says "Hello User" asynchronously.</summary>
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

    /// <summary>Says "Hello World" asynchronously.</summary>
    public async Task<string> SayHelloWorldAsync(CancellationToken cancellationToken = default) {
      var response = await this.ExecuteRequestAsync<MessageResponse>(
        new HelloRequest("world", Authorization.Open),
        cancellationToken
      ).ConfigureAwait(false);

      return response.Message;
    }
  }
}
