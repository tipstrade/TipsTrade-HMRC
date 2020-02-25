using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.Model;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class TestBase {
    protected IConfiguration Configuration { get; }

    protected ITestOutputHelper Output { get; }

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

    protected string ServerToken => Configuration.GetSection(Environment)["ServerToken"];
    #endregion

    public TestBase(ITestOutputHelper output) {
      Output = output;
      var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<TestBase>()
        ;

      Configuration = builder.Build();

      LoadUsersFromJsonFile();
    }

    protected Client GetClient() {
      var client = new Client(ClientId, ClientSecret, ServerToken, IsSandbox) {
        AntiFraud = new AntiFraud.AntiFraud() {
          ConnectionMethod = ConnectionMethod.BATCH_PROCESS_DIRECT,
          DeviceID = Configuration["AntiFraudDeviceID"],
          Screens = new Screen[] {
          new Screen() {
            ColourDepth = 32, ScalingFactor=1, Size = new Size(1920,1080) }
          },
          TimeZone = TimeZoneInfo.Local,
          UserIDs = new Dictionary<string, string>() {
            { "os", System.Environment.UserName }
          },
          VendorVersion = new Dictionary<string, string>() { { "TipsTrade.HMRC.Tests", "0.0.0.1" } },
          WindowSize = new Size(1024, 768)
        }
      };

      client.AntiFraud.PopulateLocalIPs();
      client.AntiFraud.PopulateMACAddresses();
      client.AntiFraud.PopulateUserAgent();

      // Even though the documentation states that that these are optional, the API returns an error
      client.AntiFraud.UserAgent.DeviceManufacturer = "Dell";
      client.AntiFraud.UserAgent.DeviceModel = "XPS";

      client.AntiFraud.MultiFactor = new[] {
        new MultiFactor() {
          Method = MFAMethod.AUTH_CODE,
          TimeStamp = DateTime.Now,
          UniqueReference = $"{Guid.NewGuid()}"
        }
      };

      client.AntiFraud.VendorLicenceIDs = new Dictionary<string, string>() {
        { "Example", "https://example.com" }
      };

      return client;
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
