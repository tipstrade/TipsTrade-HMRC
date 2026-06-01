using Newtonsoft.Json;
using TipsTrade.HMRC.Api.CreateTestUser;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class CreateUserTests : TestBase {
    private void TestCreateTestUserFactory<T>(string json) where T : class, ICreateTestUserRequest {
      var request = CreateTestUserFactory.CreateTestUserFull<T>();
      Assert.That(request.ServiceNames, Is.Not.Empty);

      var fromDocs = JsonConvert.DeserializeObject<T>(json);

      request.ServiceNames.Sort();
      fromDocs.ServiceNames.Sort();

      Assert.That(request.ServiceNames.Count, Is.EqualTo(fromDocs.ServiceNames.Count));
      for (int i = 0; i < request.ServiceNames.Count; i++) {
        Assert.That(request.ServiceNames[i], Is.EqualTo(fromDocs.ServiceNames[i]));
      }
    }

    private void TestCreateUser<TRequest, TUser>() where TRequest : class, ICreateTestUserRequest<TUser> where TUser : UserResultBase, new() {
      var request = CreateTestUserFactory.CreateTestUserFull<TRequest>();

      var svc = GetService<CreateTestUserService>();

      var result = svc.CreateUser(request);
      Assert.That(result, Is.Not.Null);

      foreach (var prop in result.GetType().GetProperties()) {
        var value = prop.GetValue(result);
        Assert.That(value, Is.Not.Null);
      }

      TestContext.Out.WriteLine($"Created {result.GetType()}:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    [Test]
    public void CreateTestUserFactoryAgent() {
      TestCreateTestUserFactory<CreateAgentRequest>(@"{
  ""serviceNames"": [
    ""agent-services""
  ]
}");
    }

    [Test]
    public void CreateTestUserFactoryIndividual() {
      TestCreateTestUserFactory<CreateIndividualRequest>(@"{
  ""serviceNames"": [
    ""national-insurance"",
    ""self-assessment"",
    ""mtd-income-tax"",
    ""customs-services""
  ]
}");
    }

    [Test]
    public void CreateTestUserFactoryOrganisation() {
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

    [Test]
    public void CreateTestUserFactoryPredicate() {
      CreateOrganisationRequest request;

      request = CreateTestUserFactory.CreateTestUser<CreateOrganisationRequest>(s => s == null);
      Assert.That(request.ServiceNames, Is.Empty);

      request = CreateTestUserFactory.CreateTestUser<CreateOrganisationRequest>(s => CreateOrganisationRequest.CorporationTax.Equals(s));
      Assert.That(request.ServiceNames, Has.Count.EqualTo(1));
    }

    [Test, Ignore("Skipped so we don't keep creating new users.")]
    public void CreateAgent() {
      TestCreateUser<CreateAgentRequest, AgentResult>();
    }

    [Test, Ignore("Skipped so we don't keep creating new users.")]
    public void CreateIndividual() {
      TestCreateUser<CreateIndividualRequest, IndividualResult>();
    }

    [Test, Ignore("Skipped so we don't keep creating new users.")]
    public void CreateOrganisation() {
      TestCreateUser<CreateOrganisationRequest, OrganisationResult>();
    }
  }
}
