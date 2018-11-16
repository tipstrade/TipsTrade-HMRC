using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using TipsTrade.HMRC.Api.CreateTestUser.Model;

namespace TipsTrade.HMRC.Tests {
  public class TestBase {
    protected IConfiguration Configuration { get; }

    #region State properties
    protected string AccessToken => Configuration["AccessToken"];

    protected string RefreshToken => Configuration["RefreshToken"];

    protected string State => Configuration["State"];
    #endregion

    #region User properties
    protected OrganisationResult OrganisationUser { get; }
    #endregion

    #region Client properties
    protected Client Client => new Client(ClientId, ClientSecret, ServerToken, IsSandbox);

    protected string ClientId => Configuration["ClientID"];

    protected string ClientSecret => Configuration["ClientSecret"];

    protected bool IsSandbox => true;

    protected string ServerToken => Configuration["ServerToken"];
    #endregion

    public TestBase() {
      // See https://patrickhuber.github.io/2017/07/26/avoid-secrets-in-dot-net-core-tests.html
      // From CLI: dotnet user-secrets set <Name> <Value>
      var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.tokens.json")
        .AddUserSecrets<TestBase>();

      Configuration = builder.Build();

      OrganisationUser = LoadFromJsonFile<OrganisationResult>("hmrc-user-organisation.json");
    }

    private T LoadFromJsonFile<T>(string fileName) {
      using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
        using (var reader = new StreamReader(fs)) {
          return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }
      }
    }
  }
}
