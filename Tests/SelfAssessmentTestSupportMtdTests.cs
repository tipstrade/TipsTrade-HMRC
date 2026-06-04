using System;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class SelfAssessmentTestSupportMtdTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    private string GetNiNumber() {
      return Users?.Organisation?.User?.NiNumber ?? throw new InvalidOperationException("NiNumber is not set for the user.");
    }

    private static async Task<string> SeedTestDataAsync(SelfAssessmentTestSupportMtdService svc, string niNumber) {
      var response = await svc.CreateBusinessIncomeSourceAsync(new CreateTestBusinessRequest {
        NiNumber = niNumber,
        BusinessDetails = new BusinessDetailsResult {
          TypeOfBusiness = TypeOfBusiness.SelfEmployment,
          TradingName = "My Test Business",
          BusinessAddressCountryCode = "GB",
          BusinessAddressLineOne = "15 Main Street",
          BusinessAddressPostcode = "W1 3AB"
        }
      });

      Assert.That(response, Is.Not.Null);
      Assert.That(response.Value, Is.Not.Null);

      return response.Value;
    }

    #region Main tests
    [Test]
    public async Task DeleteStatefulTestData() {
      var svc = GetService<SelfAssessmentTestSupportMtdService>();

      await SeedTestDataAsync(svc, GetNiNumber());

      var resp = await svc.DeleteStatefulTestDataAsync(GetNiNumber());

      Assert.That(resp, Is.Not.Null);
    }
    #endregion

    #region Business Income Source tests
    [Test]
    public async Task CreateBusinessIncomeSource() {
      var svc = GetService<SelfAssessmentTestSupportMtdService>();

      var response = await SeedTestDataAsync(svc, GetNiNumber());

      Assert.That(response, Is.Not.Empty);
    }
    #endregion

    #region ITSA Status tests
    [Test]
    public async Task CreateTestItsaStatus() {
      var svc = GetService<SelfAssessmentTestSupportMtdService>();
      var resp = await svc.CreateTestItsaStatusAsync(new CreateTestItsaStatusRequest {
        NiNumber = GetNiNumber(),
        TaxYear = DateTime.Now.GetTaxYear(),
        ItsaStatusDetails = [
          new ItsaStatusDetails {
            SubmittedOnDate = DateTime.Now.GetTaxYearStart().AddMonths(1),
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
