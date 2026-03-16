using System;
using System.Linq;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using TipsTrade.HMRC.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class ObligationsMtdTests : TestBase {
    public ObligationsMtdTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void GetFinalObligations() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.ObligationsMtd.GetFinalObligations(new GetFinalObligationsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        GovTestScenario = GetFinalObligationsRequest.ScenarioMultiple
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);

      var first = resp.Value.First();

      Assert.NotNull(first);

      var firstOpen = resp.Value.First(x => x.Status == ObligationStatus.Open);
      var firstFulfilled = resp.Value.First(x => x.Status == ObligationStatus.Fulfilled);

      Assert.NotNull(firstOpen);
      Assert.NotDefault(firstOpen.PeriodStartDate);
      Assert.NotDefault(firstOpen.PeriodEndDate);
      Assert.NotDefault(firstOpen.DueDate);
      Assert.Default(firstOpen.ReceivedDate);

      Assert.NotNull(firstFulfilled);
      Assert.NotDefault(firstFulfilled.PeriodStartDate);
      Assert.NotDefault(firstFulfilled.PeriodEndDate);
      Assert.NotDefault(firstFulfilled.DueDate);
      Assert.NotDefault(firstFulfilled.ReceivedDate);
    }

    [Fact]
    public void GetObligations() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var fromDate = DateTime.Today.GetTaxYearStart();
      var toDate = DateTime.Today.GetTaxYearEnd();
      var businessId = "XBIS12345678901"; // Self-employment business

      var resp = client.ObligationsMtd.GetIncomeAndExpenditureObligations(new GetObligationsRequest {
        FromDate = fromDate,
        ToDate = toDate,
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = businessId, // Self-employment business
        TypeOfBusiness = TypeOfBusiness.SelfEmployment,
        GovTestScenario = GetObligationsRequest.ScenarioDynamic
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);

      var first = resp.Value.First();

      Assert.NotNull(first);
      Assert.Equal(businessId, first.BusinessId);
      Assert.Equal(TypeOfBusiness.SelfEmployment, first.TypeOfBusiness);
      Assert.NotNull(first.Obligations);
      Assert.NotEmpty(first.Obligations);

      var firstOpen = resp.Value.First().Obligations.First(x => x.Status == ObligationStatus.Open);
      var firstFulfilled = resp.Value.First().Obligations.First(x => x.Status == ObligationStatus.Fulfilled);

      Assert.NotNull(firstOpen);
      Assert.NotDefault(firstOpen.PeriodStartDate);
      Assert.NotDefault(firstOpen.PeriodEndDate);
      Assert.NotDefault(firstOpen.DueDate);
      Assert.Default(firstOpen.ReceivedDate);

      Assert.NotNull(firstFulfilled);
      Assert.NotDefault(firstFulfilled.PeriodStartDate);
      Assert.NotDefault(firstFulfilled.PeriodEndDate);
      Assert.NotDefault(firstFulfilled.DueDate);
      Assert.NotDefault(firstFulfilled.ReceivedDate);
    }
  }
}
