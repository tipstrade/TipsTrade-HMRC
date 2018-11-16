using TipsTrade.HMRC.Api.CreateTestUser;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using Xunit;

namespace TipsTrade.HMRC.Tests {
  public  class CreateUserTests :TestBase {
    [Fact]
    public void CreateOganisation() {
      var request = CreateTestUserFactory<CreateOrganisationRequest>.CreateTestUserFull();
      Assert.NotEmpty(request.ServiceNames);
      Assert.Equal(11, request.ServiceNames.Count); // This is the number of Service Names as of v.1.0

      var organisation = Client.CreateTestUser.CreateOrganisation(request);
    }
  }
}
