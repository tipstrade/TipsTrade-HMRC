using System;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class SelfAssessmentTestSupportMtdTests : TestBase {
    public SelfAssessmentTestSupportMtdTests() {
    }

    private static string SeedTestData(SelfAssessmentTestSupportMtdService svc, string niNumber) {
      var response = svc.CreateBusinessIncomeSource(new CreateTestBusinessRequest {
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
    [Test]
    public void DeleteStatefulTestData() {
      var svc = GetService<SelfAssessmentTestSupportMtdService>();

      SeedTestData(svc, Users.Organisation.User.NiNumber);

      var resp = svc.DeleteStatefulTestData(Users.Organisation.User.NiNumber);
      Assert.That(resp, Is.Not.Null);
    }
    #endregion

    #region Business Income Source tests
    [Test]
    public void CreateBusinessIncomeSource() {
      var svc = GetService<SelfAssessmentTestSupportMtdService>();

      var response = SeedTestData(svc, Users.Organisation.User.NiNumber);

      Assert.That(response, Is.InstanceOf<string>());
    }
    #endregion

    #region ITSA Status tests
    [Test]
    public void CreateTestItsaStatus() {
      var svc = GetService<SelfAssessmentTestSupportMtdService>();

      var resp = svc.CreateTestItsaStatus(new CreateTestItsaStatusRequest {
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

      Assert.That(resp, Is.Not.Null);
    }
    #endregion
  }
}
