using Newtonsoft.Json;
using TipsTrade.HMRC.Api.CreateTestUser;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using Xunit;

namespace TipsTrade.HMRC.Tests {
  public  class CreateUserTests :TestBase {
    // Disable this by default do as not to create tonnes of users
    ///[Fact]
    public void CreateOganisation() {
      var request = CreateTestUserFactory<CreateOrganisationRequest>.CreateTestUserFull();
      Assert.NotEmpty(request.ServiceNames);

      // Compare to the request from the docs
      var fromDocs = JsonConvert.DeserializeObject<CreateOrganisationRequest>(@"{
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

      request.ServiceNames.Sort();
      fromDocs.ServiceNames.Sort();

      Assert.Equal(fromDocs.ServiceNames.Count, request.ServiceNames.Count);
      for (int i = 0; i < request.ServiceNames.Count; i++) {
        Assert.Equal(fromDocs.ServiceNames[i], request.ServiceNames[i]);
      }

      var organisation = Client.CreateTestUser.CreateOrganisation(request);

      foreach (var prop  in organisation.GetType().GetProperties()) {
        var value = prop.GetValue(organisation);
        Assert.NotNull(value);
      }
    }
  }
}
