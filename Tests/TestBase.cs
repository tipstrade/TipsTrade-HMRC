using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Extensions;
using TipsTrade.HMRC.FraudPrevention;
using TipsTrade.HMRC.FraudPrevention.ConnectionMethods;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.Tests {
  public abstract class TestBase {
    protected Mock<IHmrcAccessTokenProvider> AccessTokenProvider;

    protected Mock<IOptions<HmrcOptions>> HmrcOptionsMock;

    protected IConfiguration Configuration { get; }

    private IServiceProvider ServiceProvider { get; set; }

    #region State properties
    #endregion

    #region User properties
    protected HmrcUsers Users { get; private set; }
    #endregion

    #region Client properties
    protected string ClientId => Configuration.GetSection(Environment)["ClientID"] ?? throw new InvalidOperationException("ClientID is not configured.");

    protected string ClientSecret => Configuration.GetSection(Environment)["ClientSecret"] ?? throw new InvalidOperationException("ClientSecret is not configured.");

    private string Environment => IsSandbox ? "Sandbox" : "Production";

    protected Guid FraudPreventionDeviceId => Guid.Parse(Configuration["FraudPreventionDeviceId"] ?? throw new InvalidOperationException("FraudPreventionDeviceId is not configured."));

    protected bool IsSandbox => true;

    protected string RedirectUrl => Configuration["RedirectUrl"] ?? throw new InvalidOperationException("RedirectUrl is not configured.");
    #endregion

    public TestBase() {
      var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<TestBase>()
        ;

      Configuration = builder.Build();

      LoadUsersFromJsonFile();
    }

    protected void SetupCredentialsForOrganisation() {
      AccessTokenProvider.Setup(x => x.GetCredentialAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Users.Organisation.Tokens);
    }

    protected virtual void CustomSetup() { }

    [SetUp]
    protected void Setup() {
      CustomSetup();
    }

    /// <summary>
    /// Builds the <see cref="IServiceProvider"/> used by the test class.
    /// Called automatically by <see cref="SetupOnce"/>; invoke that from a
    /// <c>[OneTimeSetUp]</c> method in the derived class, or rely on the base
    /// <c>[OneTimeSetUp]</c> when no override is needed.
    /// </summary>
    [OneTimeSetUp]
    protected void SetupOnce() {
      var services = new ServiceCollection();
      services.AddMemoryCache();

      // Mock the access token provider.
      AccessTokenProvider = new Mock<IHmrcAccessTokenProvider>();

      // Mock the HMRC options.
      HmrcOptionsMock = new Mock<IOptions<HmrcOptions>>();
      var hmrcOptions = new global::TipsTrade.HMRC.HmrcOptions {
        FraudPreventionConfig = BuildFraudPrevention<BatchProcessDirect>(),
        ClientID = ClientId,
        ClientSecret = ClientSecret,
        IsSandbox = IsSandbox
      };
      HmrcOptionsMock.Setup(x => x.Value).Returns(hmrcOptions);
      services.AddSingleton(HmrcOptionsMock.Object);

      services.AddSingleton<Api.ApplicationTokenCache>(); // Needed for the Application Tokens
      services.AddSingleton(AccessTokenProvider.Object); // Our mocked access token provider

      services.AddHmrcOAuthService();
      services.AddHttpClient(ServiceCollectionExtensions.HttpClientName);

      services.AddBusinessDetailsMtdService();
      services.AddCreateTestUserService();
      services.AddHelloWorldService();
      services.AddIndividualCalculationsMtdService();
      services.AddObligationsMtdService();
      services.AddSelfAssessmentTestSupportMtdService();
      services.AddSelfEmploymentBusinessMtdService();
      services.AddTestFraudPreventionService();
      services.AddVatService();
      services.AddVatNumberService();

      ServiceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    protected virtual void TeardownOnce() {
      (ServiceProvider as IDisposable)?.Dispose();
      ServiceProvider = null;
    }

    /// <summary>
    /// Builds the fraud prevention headers for the tests, using the various header interfaces to populate the relevant properties.
    /// </summary>
    protected T BuildFraudPrevention<T>() where T : IFraudPrevention, new() {
      var headers = new T();

      return PopulateFraudPrevention(headers);
    }

    /// <summary>
    /// Populates the supplied fraud prevention headers with values for all properties defined by the various header interfaces.
    /// </summary>
    protected T PopulateFraudPrevention<T>(T headers) where T : IFraudPrevention {
      if (headers is IBrowserJavaScriptUserAgent browserUserAgent) {
        browserUserAgent.BrowserJavaScriptUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36";
      }

      if (headers is IDeviceId deviceId) {
        deviceId.DeviceId = FraudPreventionDeviceId;
      }

      if (headers is ILocalIps localIps) {
        localIps.PopulateLocalIps();
      }

      if (headers is IMacAddresses macAddresses) {
        macAddresses.PopulateMacAddresses();
      }

      if (headers is IMultiFactor multiFactorAuthentication) {
        multiFactorAuthentication.MultiFactor = [new MultiFactor() { TimeStamp = DateTime.UtcNow.AddMinutes(-1), Method = MFAMethod.TOTP, UniqueReference = $"{Guid.NewGuid()}" }];
      }

      if (headers is IPublicIp publicIp) {
        publicIp.PublicIp = IPAddress.Parse("1.1.1.1");
      }

      if (headers is IPublicPort publicPort) {
        publicPort.PublicPort = 12345;
      }

      if (headers is IScreens screens) {
        screens.Screens = [
          new Screen(1080,1920, 32, 1),
          new Screen(1080,1920, 32, 1)
        ];
      }

      if (headers is ITimeZone timeZone) {
        timeZone.TimeZone = TimeZoneInfo.Local;
      }

      if (headers is IUserAgent userAgent) {
        userAgent.PopulateUserAgent();

        // Even though the documentation states that these are optional, the API returns an error if they are not included.
        userAgent.UserAgent.DeviceManufacturer = "Dell";
        userAgent.UserAgent.DeviceModel = "XPS Gaming PC";
      }

      if (headers is IUserIds userIds) {
        var dictionary = new Dictionary<string, string>();

        switch (headers.ConnectionMethod) {
          case ConnectionMethod.WEB_APP_VIA_SERVER:
            dictionary.Add("account", $"{Guid.NewGuid()}@example.com");
            break;
          default:
            dictionary.Add("os", System.Environment.UserName);
            break;
        }

        userIds.UserIds = dictionary;
      }

      if (headers is IVendorForwarded vendorForwarded) {
        vendorForwarded.VendorForwarded = [new Forwarded(IPAddress.Parse("2.2.2.2"), IPAddress.Parse("1.1.1.1"))];
      }

      if (headers is IVendorLicenceIDs vendorLicenceIDs) {
        vendorLicenceIDs.VendorLicenceIDs = new Dictionary<string, string>() {
          { "Example", "https://example.com" }
        };
      }

      if (headers is IVendorProductName vendorProductName) {
        vendorProductName.VendorProductName = "TipsTrade.HMRC.Tests";
      }

      if (headers is IVendorPublicIP vendorPublicIP) {
        vendorPublicIP.VendorPublicIP = IPAddress.Parse("2.2.2.2");
      }

      if (headers is IVendorVersion vendorVersion) {
        vendorVersion.VendorVersion = new Dictionary<string, string>() {
          { "TipsTrade.HMRC.Tests", "0.0.0.1" },
          { "Another Vendor", $"{new Version(0, 0, 1, 2)}" }
        };
      }

      if (headers is IWindowSize windowSize) {
        windowSize.WindowSize = new System.Drawing.Size(1080, 1920);
      }

      return headers;
    }

    /// <summary>Resolves a DI-registered HMRC service with an optional access token.</summary>
    protected T GetService<T>() where T : HmrcServiceBase {
      return ServiceProvider.GetRequiredService<T>();
    }

    /// <summary>Creates an HMRC service instance using the supplied <see cref="HmrcOptionsMock"/>.</summary>
    protected T CreateServiceWithOptions<T>(HmrcOptions options) where T : HmrcServiceBase {
      var wrappedOptions = Options.Create(options);
      var httpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
      var accessTokenProvider = ServiceProvider.GetRequiredService<IHmrcAccessTokenProvider>();
      var tokenCache = ServiceProvider.GetRequiredService<ApplicationTokenCache>();
      var oauthService = ServiceProvider.GetRequiredService<HmrcOAuthService>();
      return (T)Activator.CreateInstance(typeof(T), wrappedOptions, httpClientFactory, accessTokenProvider, tokenCache, oauthService, null, null);
    }

    /// <summary>Resolves the <see cref="HmrcOAuthService"/> from the DI container.</summary>
    protected HmrcOAuthService GetOAuthService() => ServiceProvider.GetRequiredService<HmrcOAuthService>();

    /// <summary>Gets the <see cref="HmrcOptionsMock"/> value from the mock.</summary>
    protected HmrcOptions GetOptions() => HmrcOptionsMock.Object.Value;

    private void LoadUsersFromJsonFile() {
      Users = LoadFromJsonFile<HmrcUsers>("hmrc-users.json");
    }

    private T LoadFromJsonFile<T>(string fileName) {
      using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
        using (var reader = new StreamReader(fs)) {
          return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }
      }
    }

    #region Inner classes
    public class HmrcUsers {
      public UserToken<AgentResult> Agent { get; set; }

      public UserToken<IndividualResult> Individual { get; set; }

      public UserToken<OrganisationResult> Organisation { get; set; }
    }

    public class UserToken<TUser> where TUser : UserResultBase {
      public TUser User { get; set; }

      public TokenResponse Tokens { get; set; }
    }
    #endregion
  }
}
