using System;
using System.Collections.Generic;
using System.Linq;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model;
using TipsTrade.HMRC.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class SelfEmploymentBusinessMtdTests : TestBase {
    public SelfEmploymentBusinessMtdTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void CumulativePeriodSummaryResult_AddConsolidatedExpenses() {
      var result = new CumulativePeriodSummaryResult();

      result.AddConsolidatedExpenses(100);
      Assert.NotNull(result.PeriodExpenses);
      Assert.Equal(100, result.PeriodExpenses["consolidatedExpenses"]);
      Assert.Null(result.PeriodDisallowableExpenses);
    }

    [Fact]
    public void CumulativePeriodSummaryResult_AddDetailedExpenses() {
      var result = new CumulativePeriodSummaryResult();

      // Empty
      result.AddDetailedExpenses();
      Assert.NotEmpty(result.PeriodExpenses);
      Assert.NotEmpty(result.PeriodDisallowableExpenses);

      // Valid values
      result.AddDetailedExpenses(new Dictionary<string, decimal> {
        {"costOfGoods", 100 },
      });
      Assert.NotEmpty(result.PeriodExpenses);
      Assert.Equal(100, result.PeriodExpenses["costOfGoods"]);
      Assert.NotEmpty(result.PeriodDisallowableExpenses);
    }

    [Fact]
    public void CumulativePeriodSummaryResult_AddDetailedExpenses_Throws() {
      var result = new CumulativePeriodSummaryResult();

      // Throws on invalid key
      var ex = Assert.Throws<ArgumentException>(() => {
        result.AddDetailedExpenses(new Dictionary<string, decimal> {
          { "xxx-invalid-key", 100 }
        });
      });
      Assert.Contains("xxx-invalid-key", ex.Message);

      // Throws on consolidatedExpenses
      ex = Assert.Throws<ArgumentException>(() => {
        result.AddDetailedExpenses(new Dictionary<string, decimal> {
          { "consolidatedExpenses", 100 }
        });
      });
      Assert.Contains("consolidatedExpenses", ex.Message);

      // Doesn't alter the existing the object
      Assert.Null(result.PeriodExpenses);
      Assert.Null(result.PeriodDisallowableExpenses);
    }

    [Fact]
    public void CreateOrAmendCumulativePeriodSummary() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

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

      var resp = client.SelfEmploymentBusinessMtd.CreateOrAmendCumulativePeriodSummary(request);

      Assert.NotNull(resp);
    }

    [Fact]
    public void GetCumulativePeriodSummary() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.SelfEmploymentBusinessMtd.GetCumulativePeriodSummary(new GetCumulativePeriodSummaryRequest {
        BusinessId = "XAIS12345678910",
        GovTestScenario = GetCumulativePeriodSummaryRequest.ScenarioConsolidatedExpenses,
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = "2025-26"
      });
    }

    [Fact]
    public void ItsaJourney() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var businesses = client.BusinessDetailsMtd.ListBusinessDetails(new Api.BusinessDetailsMtd.Model.ListBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber
      });
      var businessId = businesses.Value.First().BusinessId;

      var business = client.BusinessDetailsMtd.GetBusinessDetails(new Api.BusinessDetailsMtd.Model.GetBusinessDetailsRequest {
        BusinessId = businessId,
        NiNumber = Users.Organisation.User.NiNumber
      });

      var fromDate = DateTime.Today.GetTaxYearStart();
      var toDate = DateTime.Today.GetTaxYearEnd();

      var obligations = client.ObligationsMtd.GetIncomeAndExpenditureObligations(new Api.ObligationsMtd.Model.GetObligationsRequest {
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

      var summary = client.SelfEmploymentBusinessMtd.GetCumulativePeriodSummary(new GetCumulativePeriodSummaryRequest {
        BusinessId = businessId,
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = firstOpenObligation.PeriodStartDate.GetTaxYear()
      });
    }
  }
}
