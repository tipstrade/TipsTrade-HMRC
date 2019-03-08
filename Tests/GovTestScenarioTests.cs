using System;
using System.Collections.Generic;
using System.Linq;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.Vat.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class GovTestScenarioTests : TestBase {
    public GovTestScenarioTests(ITestOutputHelper output) : base(output) {
    }

    public new Client GetClient() {
      var client = base.GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;
      return client;
    }

    public string GetPeriodKey(Client client) {
      var request = new ObligationsRequest() {
        Vrn = Users.Organisation.User.Vrn,
        Status = ObligationStatus.Fulfilled,
        DateFrom = DateTime.Today.AddYears(-1),
        DateTo = DateTime.Today
      };

      return client.Vat.GetObligations(request).Value.First().PeriodKey;
    }

    [Fact]
    public void TestGetLiabilitiesScenarios() {
      var client = GetClient();

      var request = new LiabilitiesRequest() {
        Vrn = Users.Organisation.User.Vrn,
      };

      // Multiple liability
      request.GovTestScenario = LiabilitiesRequest.ScenarioMultipleLiabilities;
      request.DateFrom = new DateTime(2017, 4, 5); // As provided by the docs
      request.DateTo = new DateTime(2017, 12, 21);
      Assert.True(client.Vat.GetLiabilities(request).Value.Count() > 1);

      // Single liability
      request.GovTestScenario = LiabilitiesRequest.ScenarioSingleLiability;
      request.DateFrom = new DateTime(2017, 1, 2);
      request.DateTo = new DateTime(2017, 2, 2);
      Assert.Single(client.Vat.GetLiabilities(request).Value);
    }

    [Fact]
    public void TestGetObligationsScenarios() {
      var client = GetClient();

      var request = new ObligationsRequest() {
        Vrn = Users.Organisation.User.Vrn,
        Status = ObligationStatus.Open
      };

      IEnumerable<string> expectedErrors;

      // Not found - expects ApiException
      request.GovTestScenario = ObligationsRequest.ScenarioNotFound;
      expectedErrors = request.GetGovTestScenarioErrors();
      var ex = Assert.Throws<ApiException>(() => client.Vat.GetObligations(request));
      Assert.Equal(expectedErrors.First(), ex.GetFirstError().Code);
    }

    [Fact]
    public void TestGetPaymentsScenarios() {
      var client = GetClient();

      var request = new PaymentsRequest() {
        Vrn = Users.Organisation.User.Vrn,
      };

      // Multiple liability
      request.GovTestScenario = PaymentsRequest.ScenarioMultiplePayment;
      request.DateFrom = new DateTime(2017, 4, 5); // As provided by the docs
      request.DateTo = new DateTime(2017, 12, 21);
      Assert.True(client.Vat.GetPayments(request).Value.Count() > 1);

      // Single liability
      request.GovTestScenario = PaymentsRequest.ScenarioSinglePayment;
      request.DateFrom = new DateTime(2017, 1, 2);
      request.DateTo = new DateTime(2017, 2, 2);
      Assert.Single(client.Vat.GetPayments(request).Value);
    }

    [Fact]
    public void TestGetReturnScenarios() {
      var client = GetClient();

      var request = new ReturnRequest() {
        Vrn = Users.Organisation.User.Vrn,
        PeriodKey = GetPeriodKey(client)
      };

      IEnumerable<string> expectedErrors;

      // Date range too large
      request.GovTestScenario = ReturnRequest.ScenarioDateRangeTooLarge;
      expectedErrors = request.GetGovTestScenarioErrors();
      var ex = Assert.Throws<ApiException>(() => client.Vat.GetReturn(request));
      Assert.Equal(expectedErrors.First(), ex.GetFirstError().Code);
    }

    [Fact]
    public void TestSubmitReturnScenarios() {
      var client = GetClient();

      var request = new SubmitRequest() {
        Vrn = Users.Organisation.User.Vrn,
        Return = new VatReturn() {
          Finalised = true,
          PeriodKey = GetPeriodKey(client)
        }
      };

      IEnumerable<string> expectedErrors;
      ApiException ex;

      // Duplicate submission
      request.GovTestScenario = SubmitRequest.ScenarioDuplicateSubmission;
      expectedErrors = request.GetGovTestScenarioErrors();
      ex = Assert.Throws<ApiException>(() => client.Vat.SubmitReturn(request));
      Assert.Equal(expectedErrors.First(), ex.GetFirstError().Code);

      // Invalid payload
      request.GovTestScenario = SubmitRequest.ScenarioInvalidPayload;
      expectedErrors = request.GetGovTestScenarioErrors();
      ex = Assert.Throws<ApiException>(() => client.Vat.SubmitReturn(request));
      Assert.Equal(expectedErrors.First(), ex.GetFirstError().Code);

      // Invalid period key
      request.GovTestScenario = SubmitRequest.ScenarioInvalidPeriodKey;
      expectedErrors = request.GetGovTestScenarioErrors();
      ex = Assert.Throws<ApiException>(() => client.Vat.SubmitReturn(request));
      Assert.Equal(expectedErrors.First(), ex.GetFirstError().Code);

      // Invalid vrn
      request.GovTestScenario = SubmitRequest.ScenarioInvalidVrn;
      expectedErrors = request.GetGovTestScenarioErrors();
      ex = Assert.Throws<ApiException>(() => client.Vat.SubmitReturn(request));
      Assert.Equal(expectedErrors.First(), ex.GetFirstError().Code);

      // Period not ended
      request.GovTestScenario = SubmitRequest.ScenarioTaxPeriodNotEnded;
      expectedErrors = request.GetGovTestScenarioErrors();
      ex = Assert.Throws<ApiException>(() => client.Vat.SubmitReturn(request));
      Assert.Equal(expectedErrors.First(), ex.GetFirstError().Code);
    }
  }
}
