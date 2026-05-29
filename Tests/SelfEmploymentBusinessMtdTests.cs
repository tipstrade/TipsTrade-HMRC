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

namespace TipsTrade.HMRC.Tests {
  public class SelfEmploymentBusinessMtdTests : TestBase {
    public SelfEmploymentBusinessMtdTests() {
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
    public void CreateOrAmendCumulativePeriodSummary() {
      var svc = GetService<SelfEmploymentBusinessMtdService>();

      var request = new AmendCumulativePeriodSummaryRequest {
        BusinessId = "XAIS12345678910",
        GovTestScenario = AmendCumulativePeriodSummaryRequest.ScenarioDefault,
        NiNumber = Users.Organisation.User.NiNumber,
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

      var resp = svc.CreateOrAmendCumulativePeriodSummary(request);

      Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public void GetCumulativePeriodSummary() {
      var svc = GetService<SelfEmploymentBusinessMtdService>();

      var resp = svc.GetCumulativePeriodSummary(new GetCumulativePeriodSummaryRequest {
        BusinessId = "XAIS12345678910",
        GovTestScenario = GetCumulativePeriodSummaryRequest.ScenarioConsolidatedExpenses,
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = "2025-26"
      });
    }

    [Test]
    public void ItsaJourney() {
      var businessDetailsSvc = GetService<BusinessDetailsMtdService>();
      var obligationsSvc = GetService<ObligationsMtdService>();
      var selfEmploymentSvc = GetService<SelfEmploymentBusinessMtdService>();

      var businesses = businessDetailsSvc.ListBusinessDetails(new Api.BusinessDetailsMtd.Model.ListBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber
      });
      var businessId = businesses.Value.First().BusinessId;

      var business = businessDetailsSvc.GetBusinessDetails(new Api.BusinessDetailsMtd.Model.GetBusinessDetailsRequest {
        BusinessId = businessId,
        NiNumber = Users.Organisation.User.NiNumber
      });

      var fromDate = DateTime.Today.GetTaxYearStart();
      var toDate = DateTime.Today.GetTaxYearEnd();

      var obligations = obligationsSvc.GetIncomeAndExpenditureObligations(new Api.ObligationsMtd.Model.GetObligationsRequest {
        FromDate = fromDate,
        ToDate = toDate,
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        TypeOfBusiness = TypeOfBusiness.SelfEmployment,
        GovTestScenario = GetObligationsRequest.ScenarioDynamic
      });

      var firstOpenObligation = obligations.Value.First().Obligations
        .Where(o => o.Status == Api.ObligationsMtd.Model.ObligationStatus.Open)
        .OrderBy(o => o.PeriodEndDate)
        .First();

      var summary = selfEmploymentSvc.GetCumulativePeriodSummary(new GetCumulativePeriodSummaryRequest {
        BusinessId = businessId,
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = firstOpenObligation.PeriodStartDate.GetTaxYear()
      });
    }
  }
}
