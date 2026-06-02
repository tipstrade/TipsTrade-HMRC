using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.BusinessDetailsMtd;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Api.ObligationsMtd;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using TipsTrade.HMRC.Extensions;
using TypeOfBusiness = TipsTrade.HMRC.Api.ObligationsMtd.Model.TypeOfBusiness;

namespace TipsTrade.HMRC.Tests {
  public class ObligationsMtdTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    private string GetNiNumber() {
      return Users?.Organisation?.User?.NiNumber ?? throw new InvalidOperationException("NiNumber is not set for the user.");
    }

    private async Task<ObligationDetail?> GetObligationsAsync(string scenario, string? businessId = null) {
      if (businessId == null) {
        var detailsSvc = GetService<BusinessDetailsMtdService>();

        var detailsResp = await detailsSvc.ListBusinessDetailsAsync(new ListBusinessDetailsRequest {
          NiNumber = GetNiNumber(),
        });

        businessId = detailsResp.Value?.FirstOrDefault()?.BusinessId;
      }

      Assert.That(businessId, Is.Not.Null, "BusinessId should not be null.");

      var svc = GetService<ObligationsMtdService>();
      var fromDate = DateTime.Today.GetTaxYearStart();
      var toDate = DateTime.Today.GetTaxYearEnd();

      var resp = await svc.GetIncomeAndExpenditureObligationsAsync(new GetObligationsRequest {
        FromDate = fromDate,
        ToDate = toDate,
        NiNumber = GetNiNumber(),
        BusinessId = businessId,
        TypeOfBusiness = TypeOfBusiness.SelfEmployment,
        GovTestScenario = scenario
      });

      TestContext.Out.WriteLine("Response:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      var first = resp.Value.First();

      Assert.That(first, Is.Not.Null);
      Assert.That(first.BusinessId, Is.EqualTo(businessId));
      Assert.That(first.TypeOfBusiness, Is.EqualTo(TypeOfBusiness.SelfEmployment));
      Assert.That(first.Obligations, Is.Not.Null);
      Assert.That(first.Obligations, Is.Not.Empty);

      return first.Obligations.FirstOrDefault();
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
        Assert.That(firstOpen.PeriodStartDate, Is.Not.Default);
        Assert.That(firstOpen.PeriodEndDate, Is.Not.Default);
        Assert.That(firstOpen.DueDate, Is.Not.Default);
        Assert.That(firstOpen.ReceivedDate, Is.Default);

        Assert.That(firstFulfilled, Is.Not.Null);
        Assert.That(firstFulfilled.PeriodStartDate, Is.Not.Default);
        Assert.That(firstFulfilled.PeriodEndDate, Is.Not.Default);
        Assert.That(firstFulfilled.DueDate, Is.Not.Default);
        Assert.That(firstFulfilled.ReceivedDate, Is.Not.Default);
      }
    }

    [Test]
    public async Task GetObligations_Cumulative() {
      var obligation = await GetObligationsAsync(GetObligationsRequest.ScenarioCumulative);

      Assert.That(obligation, Is.Not.Null);
      Assert.That(obligation.Status, Is.EqualTo(ObligationStatus.Open));
      Assert.That(obligation.PeriodStartDate, Is.Not.Default);
      Assert.That(obligation.PeriodEndDate, Is.Not.Default);
      Assert.That(obligation.DueDate, Is.Not.Default);
      Assert.That(obligation.ReceivedDate, Is.Default);
    }

    [Test]
    public async Task GetObligations_Insolvent_Trader() {
      var task = () => GetObligationsAsync(GetObligationsRequest.ScenarioInsolventTrader);

      var ex = Assert.ThrowsAsync<ApiException>(task);
    }

    [Test]
    public async Task GetObligations_NoObligationsFound() {
      var task = () => GetObligationsAsync(GetObligationsRequest.ScenarioNoObligationsFound);

      var ex = Assert.ThrowsAsync<ApiException>(task);
    }

    [Test]
    public async Task GetObligations_NotFound() {
      var task = () => GetObligationsAsync(GetObligationsRequest.ScenarioNotFound);

      var ex = Assert.ThrowsAsync<ApiException>(task);
    }

    [Test]
    public async Task GetObligations_Fulfilled() {
      var obligation = await GetObligationsAsync(GetObligationsRequest.ScenarioFulfilled, "XBIS12345678902");

      Assert.That(obligation, Is.Not.Null);
      Assert.That(obligation.Status, Is.EqualTo(ObligationStatus.Fulfilled));
      Assert.That(obligation.PeriodStartDate, Is.Not.Default);
      Assert.That(obligation.PeriodEndDate, Is.Not.Default);
      Assert.That(obligation.DueDate, Is.Not.Default);
      Assert.That(obligation.ReceivedDate, Is.Not.Default);
    }

    [Test]
    public async Task GetObligations_Open() {
      var obligation = await GetObligationsAsync(GetObligationsRequest.ScenarioOpen, "XBIS12345678903");

      Assert.That(obligation, Is.Not.Null);
      Assert.That(obligation.Status, Is.EqualTo(ObligationStatus.Open));
      Assert.That(obligation.PeriodStartDate, Is.Not.Default);
      Assert.That(obligation.PeriodEndDate, Is.Not.Default);
      Assert.That(obligation.DueDate, Is.Not.Default);
      Assert.That(obligation.ReceivedDate, Is.Default);
    }
  }
}
