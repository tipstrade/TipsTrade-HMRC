using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using TipsTrade.HMRC.Api.CreateTestUser.Model;

namespace TipsTrade.HMRC.Tests {
  public class TestBase {
    // Move these into appsetings...
    protected const string AccessToken = "abfc94eb90f7179f2f2b32ed14615bc";
    protected const string State = "4f00d15e-de25-4796-999f-266ea4429889";

    // Move this into a string typed object
    protected OrganisationResult OrganisationUser { get; }

    protected IConfiguration Configuration { get; }

    protected string ClientId => Configuration["ClientID"];

    protected string ClientSecret => Configuration["ClientSecret"];

    protected bool IsSandbox => true;

    protected string ServerToken => Configuration["ServerToken"];

    protected Client Client => new Client(ClientId, ClientSecret, ServerToken, IsSandbox);

    public TestBase() {
      // See https://patrickhuber.github.io/2017/07/26/avoid-secrets-in-dot-net-core-tests.html
      // From CLI: dotnet user-secrets set <Name> <Value>
      var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
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
