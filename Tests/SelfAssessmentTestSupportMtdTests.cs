using System;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model;
using TipsTrade.HMRC.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class SelfAssessmentTestSupportMtdTests : TestBase {
    public SelfAssessmentTestSupportMtdTests(ITestOutputHelper output) : base(output) {
    }

    private static string SeedTestData(Client client, string niNumber) {
      var response = client.SelfAssessmentTestSupportMtd.CreateBusinessIncomeSource(new CreateTestBusinessRequest {
        NiNumber = niNumber,
        BusinessDetails = new BusinessDetailsResult {
          TypeOfBusiness = TypeOfBusiness.SelfEmployment,
          TradingName = "My Test Business",
          BusinessAddressCountryCode = "GB",
          BusinessAddressLineOne = "15 Main Street",
          BusinessAddressPostcode = "W1 3AB"
        }
      });

      return response.Value;
    }

    #region Main tests
    [Fact]
    public void DeleteStatefulTestData() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      SeedTestData(client, Users.Organisation.User.NiNumber);

      var resp = client.SelfAssessmentTestSupportMtd.DeleteStatefulTestData(Users.Organisation.User.NiNumber);
      Assert.NotNull(resp);
    }
    #endregion

    #region Business Income Source tests
    [Fact]
    public void CreateBusinessIncomeSource() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var response = SeedTestData(client, Users.Organisation.User.NiNumber);

      Assert.IsType<string>(response);
    }
    #endregion

    #region ITSA Status tests
    [Fact]
    public void CreateTestItsaStatus() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.SelfAssessmentTestSupportMtd.CreateTestItsaStatus(new CreateTestItsaStatusRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Now.GetTaxYear(),
        ItsaStatusDetails = [
          new ItsaStatusDetails {
            SubmittedOnDate = DateTime.Now.GetTaxYearStart().AddMonths(-1),
            Status = ItsaStatus.MtdMandated,
            StatusReason = ItsaStatusReasons.SignUpReturnAvailable
          }
        ]
      });

      Assert.NotNull(resp);
    }
    #endregion
  }
}
