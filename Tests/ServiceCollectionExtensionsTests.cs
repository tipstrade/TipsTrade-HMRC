using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Extensions;

namespace TipsTrade.HMRC.Tests {
  public class ServiceCollectionExtensionsTests {
    ServiceCollection Services { get; set; }

    Mock<IHmrcAccessTokenProvider> HmrcAccessTokenProviderMock { get; set; }
    Mock<Action<HmrcOptions>> ConfigureOptionsMock { get; set; }
    Mock<Action<HttpClient>> ConfigureHttpClientMock { get; set; }

    [OneTimeSetUp]
    public void SetupOnce() {
      HmrcAccessTokenProviderMock = new Mock<IHmrcAccessTokenProvider>();
      ConfigureOptionsMock = new Mock<Action<HmrcOptions>>();
      ConfigureHttpClientMock = new Mock<Action<HttpClient>>();
    }

    [SetUp]
    public void Setup() {
      Services = new ServiceCollection();

      HmrcAccessTokenProviderMock.Reset();
      ConfigureOptionsMock.Reset();
      ConfigureHttpClientMock.Reset();
    }

    [Test]
    public void AddHmrc_CallsConfigureOptions() {
      ConfigureOptionsMock
        .Setup(c => c(It.IsAny<HmrcOptions>()))
        .Callback<HmrcOptions>(options => {
          options.ClientId = "configured_client_id";
        });

      Services.AddHmrc<DummyAccessTokenProvider>(ConfigureOptionsMock.Object);

      var serviceProvider = Services.BuildServiceProvider();
      var options = serviceProvider.GetRequiredService<IOptions<HmrcOptions>>();

      Assert.That(options.Value.ClientId, Is.EqualTo("configured_client_id"));
      ConfigureOptionsMock.Verify(c => c(It.IsAny<HmrcOptions>()), Times.Once);
    }

    [Test]
    public void AddHmrcHttpClient_CallsConfigure() {
      // Configure the mock to add a timeout of 123 seconds to the HttpClient, so we can verify that the configuration is applied.
      ConfigureHttpClientMock
        .Setup(c => c(It.IsAny<HttpClient>()))
        .Callback<HttpClient>(client => client.Timeout = TimeSpan.FromSeconds(123));

      Services.AddHmrcHttpClient(ConfigureHttpClientMock.Object);

      var serviceProvider = Services.BuildServiceProvider();
      var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
      var client = factory.CreateClient(ServiceCollectionExtensions.HttpClientName);

      Assert.That(client.Timeout, Is.EqualTo(TimeSpan.FromSeconds(123)));
      ConfigureHttpClientMock.Verify(c => c(It.IsAny<HttpClient>()), Times.Once);
    }

    public class DummyAccessTokenProvider : IHmrcAccessTokenProvider {
      public Task<TokenResponse> GetCredentialAsync(string key, CancellationToken cancellationToken = default) {
        return Task.FromResult(new TokenResponse {
          AccessToken = "dummy_access",
          ExpiresIn = 3600,
          RefreshToken = "dummy_refresh",
          Scope = "dummy_scope",
          TokenType = "Bearer"
        });
      }

      public Task SetCredentialAsync(string key, TokenResponse credential, CancellationToken cancellationToken = default) {
        return Task.CompletedTask;
      }
    }
  }
}