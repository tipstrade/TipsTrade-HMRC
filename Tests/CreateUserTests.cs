using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using TipsTrade.HMRC.Api.CreateTestUser;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class CreateUserTests : TestBase {
    public CreateUserTests(ITestOutputHelper output) : base(output) {
    }

    private void TestCreateTestUserFactory<T>(string json) where T : class, ICreateTestUserRequest {
      var request = CreateTestUserFactory<T>.CreateTestUserFull();
      Assert.NotEmpty(request.ServiceNames);

      var fromDocs = JsonConvert.DeserializeObject<T>(json);

      request.ServiceNames.Sort();
      fromDocs.ServiceNames.Sort();

      Assert.Equal(fromDocs.ServiceNames.Count, request.ServiceNames.Count);
      for (int i = 0; i < request.ServiceNames.Count; i++) {
        Assert.Equal(fromDocs.ServiceNames[i], request.ServiceNames[i]);
      }
    }

    private void TestCreateUser<TRequest, TUser>() where TRequest : class, ICreateTestUserRequest where TUser : UserResultBase {
      var request = CreateTestUserFactory<TRequest>.CreateTestUserFull();

      var client = GetClient();

      var method = client.CreateTestUser.GetType().GetMethods()
        .Where(m => {
          return m.Name.Equals(nameof(Client.CreateTestUser.CreateUser)) && m.GetParameters().All(p => {
            return p.ParameterType == typeof(TRequest);
          });
        }).First();
      var result = (TUser)method.Invoke(client.CreateTestUser, new object[] { request });
      Assert.NotNull(result);

      foreach (var prop in result.GetType().GetProperties()) {
        var value = prop.GetValue(result);
        Assert.NotNull(value);
      }

      Output.WriteLine($"Created {result.GetType()}:");
      Output.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    [Fact]
    public void TestCreateTestUserFactoryAgent() {
      TestCreateTestUserFactory<CreateAgentRequest>(@"{
  ""serviceNames"": [
    ""agent-services""
  ]
}");
    }

    [Fact]
    public void TestCreateTestUserFactoryIndividual() {
      TestCreateTestUserFactory<CreateIndividualRequest>(@"{
  ""serviceNames"": [
    ""national-insurance"",
    ""self-assessment"",
    ""mtd-income-tax"",
    ""customs-services""
  ]
}");
    }

    [Fact]
    public void TestCreateTestUserFactoryOrganisation() {
      TestCreateTestUserFactory<CreateOrganisationRequest>(@"{
  ""serviceNames"": [
    ""corporation-tax"",
    ""paye-for-employers"",
    ""submit-vat-returns"",
    ""national-insurance"",
    ""self-assessment"",
    ""mtd-income-tax"",
    ""mtd-vat"",
    ""lisa"",
    ""secure-electronic-transfer"",
    ""relief-at-source"",
    ""customs-services""
  ]
}");
    }

    [Fact]
    void TestCreateTestUserFactoryPredicate() {
      CreateOrganisationRequest request;

      request = CreateTestUserFactory<CreateOrganisationRequest>.CreateTestUser(s => s == null);
      Assert.Empty(request.ServiceNames);

      request = CreateTestUserFactory<CreateOrganisationRequest>.CreateTestUser(s => CreateOrganisationRequest.CorporationTax.Equals(s));
      Assert.Single(request.ServiceNames);
    }

    [Fact(Skip = "Skipped so we don't keep creating new users.")]
    public void TestCreateAgent() {
      TestCreateUser<CreateAgentRequest, AgentResult>();
    }

    [Fact(Skip = "Skipped so we don't keep creating new users.")]
    public void TestCreateIndividual() {
      TestCreateUser<CreateIndividualRequest, IndividualResult>();
    }

    [Fact(Skip = "Skipped so we don't keep creating new users.")]
    public void TestCreateOganisation() {
      TestCreateUser<CreateOrganisationRequest, OrganisationResult>();
    }
  }
}
