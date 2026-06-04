using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Api.Vat;
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
    public async Task AddHmrc_CallsConfigureOptions() {
      ConfigureOptionsMock
        .Setup(c => c(It.IsAny<HmrcOptions>()))
        .Callback<HmrcOptions>(options => {
          options.ClientId = "configured_client_id";
        });

      Services.AddHmrc<MockedAccessTokenProvider>(ConfigureOptionsMock.Object);

      var serviceProvider = Services.BuildServiceProvider();
      var optionsProvider = serviceProvider.GetRequiredService<IHmrcOptionsProvider>();
      var options = await optionsProvider.GetOptionsAsync();

      Assert.That(options.ClientId, Is.EqualTo("configured_client_id"));
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

    [Test]
    public async Task TenantProvider_IsCalled() {
      var expectedTenantId = "tenant_id";
      var expectedTokenResponse = new TokenResponse {
        AccessToken = "access_token",
        CreatedAt = DateTime.UtcNow,
        ExpiresIn = 3600,
        RefreshToken = "refresh_token",
        Scope = "scope",
        TokenType = "Bearer"
      };

      var tenantProviderMock = new Mock<IHmrcTenantProvider>();
      tenantProviderMock.Setup(m => m.GetTenantAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expectedTenantId);

      Services.AddScoped(_ => tenantProviderMock.Object);
      Services.AddHmrc<MockedAccessTokenProvider>(ConfigureOptionsMock.Object);

      var serviceProvider = Services.BuildServiceProvider();
      var accessTokenProvider = (MockedAccessTokenProvider)serviceProvider.GetRequiredService<IHmrcAccessTokenProvider>();
      var oauthService = serviceProvider.GetRequiredService<HmrcOAuthService>();

      accessTokenProvider.GetCredentialAsyncMock.Setup(m => m(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((string key, CancellationToken cancellationToken) => {
          // Verify that the tenant provider is called to get the tenant ID, and that the tenant ID is included in the key used to get the credential.
          Assert.That(key, Does.Contain(expectedTenantId));

          return expectedTokenResponse;
        });

      var resp = await oauthService.CheckUserAuthenticationStateAsync();

      Assert.That(resp.IsValid, Is.True);
      tenantProviderMock.Verify(m => m.GetTenantAsync(It.IsAny<CancellationToken>()), Times.Once);
      accessTokenProvider.GetCredentialAsyncMock.Verify(m => m(It.Is<string>(k => k == expectedTenantId), It.IsAny<CancellationToken>()), Times.Once);

    }

    public class MockedAccessTokenProvider : IHmrcAccessTokenProvider {
      public Mock<Func<string, CancellationToken, Task<TokenResponse?>>> GetCredentialAsyncMock { get; } = new Mock<Func<string, CancellationToken, Task<TokenResponse?>>>();

      public Mock<Func<string, TokenResponse, CancellationToken, Task>> SetCredentialAsyncMock { get; } = new Mock<Func<string, TokenResponse, CancellationToken, Task>>();

      public Task<TokenResponse?> GetCredentialAsync(string key, CancellationToken cancellationToken = default) {
        return GetCredentialAsyncMock.Object(key, cancellationToken);
      }

      public Task SetCredentialAsync(string key, TokenResponse credential, CancellationToken cancellationToken = default) {
        return SetCredentialAsyncMock.Object(key, credential, cancellationToken);
      }
    }
  }
}