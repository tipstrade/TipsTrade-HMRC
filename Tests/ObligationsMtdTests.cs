using System;
using System.Linq;
using TipsTrade.HMRC.Api.ObligationsMtd;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class ObligationsMtdTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    private string GetNiNumber() {
      return Users?.Organisation?.User?.NiNumber ?? throw new InvalidOperationException("NiNumber is not set for the user.");
    }

    [Test]
    public async Task GetFinalObligations() {
      var svc = GetService<ObligationsMtdService>();

      var resp = await svc.GetFinalObligationsAsync(new GetFinalObligationsRequest {
        NiNumber = GetNiNumber(),
        GovTestScenario = GetFinalObligationsRequest.ScenarioMultiple
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      var first = resp.Value.First();

      Assert.That(first, Is.Not.Null);

      var firstOpen = resp.Value.First(x => x.Status == ObligationStatus.Open);
      var firstFulfilled = resp.Value.First(x => x.Status == ObligationStatus.Fulfilled);

      using (Assert.EnterMultipleScope()) {
        Assert.That(firstOpen, Is.Not.Null);
        AssertExtensions.NotDefault(firstOpen.PeriodStartDate);
        AssertExtensions.NotDefault(firstOpen.PeriodEndDate);
        AssertExtensions.NotDefault(firstOpen.DueDate);
        AssertExtensions.Default(firstOpen.ReceivedDate);

        Assert.That(firstFulfilled, Is.Not.Null);
        AssertExtensions.NotDefault(firstFulfilled.PeriodStartDate);
        AssertExtensions.NotDefault(firstFulfilled.PeriodEndDate);
        AssertExtensions.NotDefault(firstFulfilled.DueDate);
        AssertExtensions.NotDefault(firstFulfilled.ReceivedDate);
      }
    }

    [Test]
    public async Task GetObligations() {
      var svc = GetService<ObligationsMtdService>();
      var fromDate = DateTime.Today.GetTaxYearStart();
      var toDate = DateTime.Today.GetTaxYearEnd();
      var businessId = "XBIS12345678901"; // Self-employment business
      var resp = await svc.GetIncomeAndExpenditureObligationsAsync(new GetObligationsRequest {
        FromDate = fromDate,
        ToDate = toDate,
        NiNumber = GetNiNumber(),
        BusinessId = businessId, // Self-employment business
        TypeOfBusiness = TypeOfBusiness.SelfEmployment,
        GovTestScenario = GetObligationsRequest.ScenarioDynamic
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      var first = resp.Value.First();

      Assert.That(first, Is.Not.Null);
      Assert.That(first.BusinessId, Is.EqualTo(businessId));
      Assert.That(first.TypeOfBusiness, Is.EqualTo(TypeOfBusiness.SelfEmployment));
      Assert.That(first.Obligations, Is.Not.Null);
      Assert.That(first.Obligations, Is.Not.Empty);

      var firstOpen = first.Obligations.FirstOrDefault(x => x.Status == ObligationStatus.Open);
      var firstFulfilled = first.Obligations.FirstOrDefault(x => x.Status == ObligationStatus.Fulfilled);

      Assert.That(firstOpen, Is.Not.Null);
      AssertExtensions.NotDefault(firstOpen.PeriodStartDate);
      AssertExtensions.NotDefault(firstOpen.PeriodEndDate);
      AssertExtensions.NotDefault(firstOpen.DueDate);
      AssertExtensions.Default(firstOpen.ReceivedDate);

      Assert.That(firstFulfilled, Is.Not.Null);
      AssertExtensions.NotDefault(firstFulfilled.PeriodStartDate);
      AssertExtensions.NotDefault(firstFulfilled.PeriodEndDate);
      AssertExtensions.NotDefault(firstFulfilled.DueDate);
      AssertExtensions.NotDefault(firstFulfilled.ReceivedDate);
    }
  }
}
