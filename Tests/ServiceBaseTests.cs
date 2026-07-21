using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Extensions;

namespace TipsTrade.HMRC.Tests {
  public class ServiceBaseTests {
    private Mock<IHmrcAccessTokenProvider> HmrcAccessTokenProvider { get; } = new Mock<IHmrcAccessTokenProvider>();

    private Mock<IHmrcOptionsProvider> HmrcOptionsProvider { get; } = new Mock<IHmrcOptionsProvider>();

    private Mock<IHmrcTenantProvider> HmrcTenantProvider { get; } = new Mock<IHmrcTenantProvider>();

    private ServiceProvider ServiceProvider { get; set; }

    private IServiceScope Scope { get; set; }

    [OneTimeSetUp]
    public void OneTimeSetup() {
      var services = new ServiceCollection();

      services.AddHmrcHttpClient();
      services.AddSingleton<Api.ApplicationTokenCache>();
      services.AddScoped<IHmrcAccessTokenProvider>(sp => HmrcAccessTokenProvider.Object);
      services.AddScoped<IHmrcOptionsProvider>(sp => HmrcOptionsProvider.Object);
      services.AddScoped<IHmrcTenantProvider>(sp => HmrcTenantProvider.Object);
      services.AddScoped<HmrcServiceBase>(sp =>
        new Mock<HmrcServiceBase>(
          sp.GetRequiredService<IHttpClientFactory>(),
          sp.GetRequiredService<IHmrcOptionsProvider>(),
          sp.GetRequiredService<IHmrcAccessTokenProvider>(),
          sp.GetRequiredService<Api.ApplicationTokenCache>(),
          sp.GetRequiredService<HmrcOAuthService>(),
          sp.GetService<IHmrcTenantProvider>(),
          null
        ).Object);
      services.AddHmrcOAuthService();

      ServiceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() {
      ServiceProvider.Dispose();
    }

    [SetUp]
    public void Setup() {
      HmrcAccessTokenProvider.Reset();
      HmrcTenantProvider.Reset();
      HmrcOptionsProvider.Reset();

      Scope = ServiceProvider.CreateScope();
    }

    [TearDown]
    public void TearDown() {
      Scope.Dispose();
    }

    [Test]
    public void GetService_ShouldReturnServiceInstance() {
      var service = Scope.ServiceProvider.GetService<HmrcServiceBase>();

      Assert.That(service, Is.Not.Null);
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task GetTenantAsync_ShouldReturnExpectedTenant(string? tenant) {
      var expected = "test-tenant";
      HmrcTenantProvider.Setup(tp => tp.GetTenantAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(expected);

      var service = Scope.ServiceProvider.GetRequiredService<HmrcServiceBase>();
      service.Tenant = tenant;
      var actual = await service.GetTenantAsync(default);

      Assert.That(actual, Is.EqualTo(expected));
      HmrcTenantProvider.Verify(tp => tp.GetTenantAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetTenantAsync_ShouldReturnOverriddenTenant() {
      var expected = "overridden-tenant";
      var service = Scope.ServiceProvider.GetRequiredService<HmrcServiceBase>();
      service.Tenant = expected;
      var actual = await service.GetTenantAsync(default);

      Assert.That(actual, Is.EqualTo(expected));
      HmrcTenantProvider.Verify(tp => tp.GetTenantAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
  }
}
