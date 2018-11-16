using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TipsTrade.HMRC.Api.CreateTestUser.Model;

namespace TipsTrade.HMRC.Tests {
  public class TestBase {
    // Move these into appsetings...
    protected const string AccessToken = "abfc94eb90f7179f2f2b32ed14615bc";
    protected const string State = "4f00d15e-de25-4796-999f-266ea4429889";

    // Move this into a string typed object
    protected OrganisationResult OrganisationUsers => Configuration.GetSection("hmrc-organisation").Get<OrganisationResult>();

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
        .AddJsonFile("appsettings.hmrc.json")
        .AddUserSecrets<TestBase>();

      Configuration = builder.Build();
    }
  }
}
