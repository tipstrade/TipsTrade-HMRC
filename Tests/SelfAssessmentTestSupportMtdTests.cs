using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class SelfAssessmentTestSupportMtdTests : TestBase {
    public SelfAssessmentTestSupportMtdTests(ITestOutputHelper output) : base(output) {
    }

    #region Main tests
    [Fact]
    public void DeleteStatefulTestData() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.SelfAssessmentTestSupportMtd.DeleteStatefulTestData(Users.Organisation.User.NiNumber);
      Assert.NotNull(resp);
    }
    #endregion

    #region Business Income Source tests
    [Fact]
    public void CreateTestBusinessResponse() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var response = client.SelfAssessmentTestSupportMtd.CreateBusinessIncomeSource(new CreateTestBusinessRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessDetails = new BusinessDetailsResult {
          TypeOfBusiness = TypeOfBusiness.SelfEmployment,
          TradingName = "My Test Business",
          BusinessAddressCountryCode = "GB",
          BusinessAddressLineOne = "15 Main Street",
          BusinessAddressPostcode = "W1 3AB"
        }
      });

      Assert.NotNull(response);
      Assert.IsType<string>(response.Value);
    }
    #endregion
  }
}
