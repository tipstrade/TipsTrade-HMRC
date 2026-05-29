using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Extensions;

namespace TipsTrade.HMRC.Tests {
  public abstract class TestBase {
    protected Mock<IHmrcAccessTokenProvider> AccessTokenProvider;

    protected IConfiguration Configuration { get; }

    private IServiceProvider ServiceProvider { get; set; }

    #region State properties
    protected string State => Configuration["State"];
    #endregion

    #region User properties
    protected HmrcUsers Users { get; private set; }
    #endregion

    #region Client properties
    protected string ClientId => Configuration.GetSection(Environment)["ClientID"];

    protected string ClientSecret => Configuration.GetSection(Environment)["ClientSecret"];

    private string Environment => IsSandbox ? "Sandbox" : "Production";

    protected bool IsSandbox => true;

    protected string RedirectUrl => Configuration["RedirectUrl"];
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

      services.Configure((Action<HmrcOptions>)ConfigureHmrc);

      services.AddSingleton<Api.ApplicationTokenCache>(); // Needed for the Application Tokens
      services.AddSingleton(AccessTokenProvider.Object); // Our mocked access token provider

      services.AddHmrcOAuthService();
      services.AddHttpClient(Api.HmrcServiceBase.HttpClientName);

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

    protected virtual void ConfigureHmrc(HmrcOptions options) {
      var antiFraud = BuildAntiFraud();
      options.AntiFraud = antiFraud;
      options.ClientID = ClientId;
      options.ClientSecret = ClientSecret;
      options.IsSandbox = IsSandbox;
    }

    /// <summary>Builds and configures the <see cref="AntiFraud.AntiFraud"/> instance used for all requests.</summary>
    protected AntiFraud.AntiFraud BuildAntiFraud() {
      var antiFraud = new AntiFraud.AntiFraud() {
        ConnectionMethod = ConnectionMethod.BATCH_PROCESS_DIRECT,
        DeviceID = Configuration["AntiFraudDeviceID"],
        Screens = new Screen[] {
          new Screen() {
            ColourDepth = 32, ScalingFactor = 1, Size = new Size(1920, 1080) }
        },
        TimeZone = TimeZoneInfo.Local,
        UserIDs = new Dictionary<string, string>() {
          { "os", System.Environment.UserName }
        },
        VendorProductName = "TipsTrade.HMRC.Tests",
        VendorVersion = new Dictionary<string, string>() {
          { "TipsTrade.HMRC.Tests", "0.0.0.1" },
          { "Another Vendor", $"{new Version(0, 0, 1, 2)}" }
        },
        WindowSize = new Size(1024, 768)
      };

      antiFraud.PopulateLocalIPs();
      antiFraud.PopulateMACAddresses();
      antiFraud.PopulateUserAgent();
      antiFraud.VendorForwarded = new Forwarded[] {
        new Forwarded { By = System.Net.IPAddress.Parse("8.8.8.8"), For = System.Net.IPAddress.Parse("fe80::21a6:9255:4c0b:78e4%14") }
      };

      // Even though the documentation states that these are optional, the API returns an error
      antiFraud.UserAgent.DeviceManufacturer = "Dell";
      antiFraud.UserAgent.DeviceModel = "XPS Gaming PC";

      antiFraud.MultiFactor = new[] {
        new MultiFactor() {
          Method = MFAMethod.AUTH_CODE,
          TimeStamp = DateTime.Now,
          UniqueReference = $"{Guid.NewGuid()}"
        }
      };

      antiFraud.VendorLicenceIDs = new Dictionary<string, string>() {
        { "Example", "https://example.com" }
      };

      return antiFraud;
    }

    /// <summary>Resolves a DI-registered HMRC service with an optional access token.</summary>
    protected T GetService<T>() where T : HmrcServiceBase {
      return ServiceProvider.GetRequiredService<T>();
    }

    /// <summary>Creates an HMRC service instance using the supplied <see cref="HmrcOptions"/>.</summary>
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

    /// <summary>Creates an <see cref="HmrcOptions"/> snapshot from the current test configuration.</summary>
    protected HmrcOptions GetOptions() {
      return new HmrcOptions {
        AntiFraud = BuildAntiFraud(),
        ClientID = ClientId,
        ClientSecret = ClientSecret,
        IsSandbox = IsSandbox
      };
    }

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
