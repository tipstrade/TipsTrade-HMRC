using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
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

    protected Client GetClient() => new Client(ClientId, ClientSecret, ServerToken, IsSandbox);

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
