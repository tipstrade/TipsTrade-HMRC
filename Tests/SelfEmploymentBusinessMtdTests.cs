using System;
using System.Collections.Generic;
using System.Linq;
using TipsTrade.HMRC.Api.BusinessDetailsMtd;
using TipsTrade.HMRC.Api.ObligationsMtd;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class SelfEmploymentBusinessMtdTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    private string GetNiNumber() {
      return Users?.Organisation?.User?.NiNumber ?? throw new InvalidOperationException("NiNumber is not set for the user.");
    }

    [Test]
    public void CumulativePeriodSummaryResult_AddConsolidatedExpenses() {
      var result = new CumulativePeriodSummaryResult();

      result.AddConsolidatedExpenses(100);
      Assert.That(result.PeriodExpenses, Is.Not.Null);
      Assert.That(result.PeriodExpenses["consolidatedExpenses"], Is.EqualTo(100));
      Assert.That(result.PeriodDisallowableExpenses, Is.Null);
    }

    [Test]
    public void CumulativePeriodSummaryResult_AddDetailedExpenses() {
      var result = new CumulativePeriodSummaryResult();

      // Empty
      result.AddDetailedExpenses();
      Assert.That(result.PeriodExpenses, Is.Not.Empty);
      Assert.That(result.PeriodDisallowableExpenses, Is.Not.Empty);

      // Valid values
      result.AddDetailedExpenses(new Dictionary<string, decimal> {
        {"costOfGoods", 100 },
      });
      Assert.That(result.PeriodExpenses, Is.Not.Empty);
      Assert.That(result.PeriodExpenses["costOfGoods"], Is.EqualTo(100));
      Assert.That(result.PeriodDisallowableExpenses, Is.Not.Empty);
    }

    [Test]
    public void CumulativePeriodSummaryResult_AddDetailedExpenses_Throws() {
      var result = new CumulativePeriodSummaryResult();

      // Throws on invalid key
      var ex = Assert.Throws<ArgumentException>((Action)(() => {
        result.AddDetailedExpenses(new Dictionary<string, decimal> {
          { "xxx-invalid-key", 100 }
        });
      }));
      Assert.That(ex.Message, Does.Contain("xxx-invalid-key"));

      // Throws on consolidatedExpenses
      ex = Assert.Throws<ArgumentException>((Action)(() => {
        result.AddDetailedExpenses(new Dictionary<string, decimal> {
          { "consolidatedExpenses", 100 }
        });
      }));
      Assert.That(ex.Message, Does.Contain("consolidatedExpenses"));

      // Doesn't alter the existing the object
      Assert.That(result.PeriodExpenses, Is.Null);
      Assert.That(result.PeriodDisallowableExpenses, Is.Null);
    }

    [Test]
    public async Task CreateOrAmendCumulativePeriodSummary() {
      var svc = GetService<SelfEmploymentBusinessMtdService>();
      var request = new AmendCumulativePeriodSummaryRequest {
        BusinessId = "XAIS12345678910",
        GovTestScenario = AmendCumulativePeriodSummaryRequest.ScenarioDefault,
        NiNumber = GetNiNumber(),
        TaxYear = "2025-26",
        Summary = new CumulativePeriodSummaryResult {
          PeriodDates = new PeriodDates {
            PeriodStartDate = new DateTime(2025, 4, 6),
            PeriodEndDate = new DateTime(2026, 4, 5)
          },
          PeriodIncome = new Income {
          }
        }.AddConsolidatedExpenses(100)
      };
      var resp = await svc.CreateOrAmendCumulativePeriodSummaryAsync(request);

      Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task GetCumulativePeriodSummary() {
      var svc = GetService<SelfEmploymentBusinessMtdService>();
      var resp = await svc.GetCumulativePeriodSummaryAsync(new GetCumulativePeriodSummaryRequest {
        BusinessId = "XAIS12345678910",
        GovTestScenario = GetCumulativePeriodSummaryRequest.ScenarioConsolidatedExpenses,
        NiNumber = GetNiNumber(),
        TaxYear = "2025-26"
      });
    }

    [Test]
    public async Task ItsaJourney() {
      var businessDetailsSvc = GetService<BusinessDetailsMtdService>();
      var obligationsSvc = GetService<ObligationsMtdService>();
      var selfEmploymentSvc = GetService<SelfEmploymentBusinessMtdService>();
      var businesses = await businessDetailsSvc.ListBusinessDetailsAsync(new Api.BusinessDetailsMtd.Model.ListBusinessDetailsRequest {
        NiNumber = GetNiNumber()
      });

      Assert.That(businesses.Value, Is.Not.Null);
      Assert.That(businesses.Value, Is.Not.Empty);

      var businessId = businesses.Value.First().BusinessId;
      Assert.That(businessId, Is.Not.Null);

      var business = await businessDetailsSvc.GetBusinessDetailsAsync(new Api.BusinessDetailsMtd.Model.GetBusinessDetailsRequest {
        BusinessId = businessId,
        NiNumber = GetNiNumber()
      });

      var fromDate = DateTime.Today.GetTaxYearStart();
      var toDate = DateTime.Today.GetTaxYearEnd();

      var obligations = await obligationsSvc.GetIncomeAndExpenditureObligationsAsync(new Api.ObligationsMtd.Model.GetObligationsRequest {
        FromDate = fromDate,
        ToDate = toDate,
        NiNumber = GetNiNumber(),
        BusinessId = "XBIS12345678901", // Self-employment business
        TypeOfBusiness = TypeOfBusiness.SelfEmployment,
        GovTestScenario = GetObligationsRequest.ScenarioDynamic
      });

      Assert.That(obligations.Value, Is.Not.Null);
      Assert.That(obligations.Value, Is.Not.Empty);

      var firstOpenObligation = obligations.Value.FirstOrDefault()?.Obligations?
        .Where(o => o.Status == ObligationStatus.Open)
        .OrderBy(o => o.PeriodEndDate)
        .FirstOrDefault();

      Assert.That(firstOpenObligation, Is.Not.Null, "No open obligations found for the business.");

      var summary = await selfEmploymentSvc.GetCumulativePeriodSummaryAsync(new GetCumulativePeriodSummaryRequest {
        BusinessId = businessId,
        NiNumber = GetNiNumber(),
        TaxYear = firstOpenObligation.PeriodStartDate.GetTaxYear()
      });
    }
  }
}
