using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>The API that exposes VAT functions.</summary>
  public class VatApi : IApi, IClient, IRequiresAntiFraud {
    private const string FuelScaleChargesResource = "Resources.FuelScaleCharges.json";

    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <summary>The description of the API.</summary>
    public string Description => "An API for providing VAT data.";

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public bool IsStable => false;

    /// <summary>The relative location of the API.</summary>
    public string Location => "organisations/vat";

    /// <summary>The name of the API.</summary>
    public string Name => "VAT (MTD) API";

    /// <summary>The version of the API that the client should target.</summary>
    public string Version => "1.0";
    #endregion

    #region Methods
    /// <summary>
    /// Gets the fuel scale charge from the live HMRC website.
    /// Deprectated, used the <see cref="GetFuelScaleChargeFromCO2Live(DateTime, VatPeriod, int)"/> method instead.
    /// </summary>
    /// <param name="date">The accounting period for which the scale charge should be retrieved.</param>
    /// <param name="periodLength">The length of the VAT period in months (1, 3, 12).</param>
    /// <param name="co2">The CO2 emmissions (g/km) of the vehicle.</param>
    [Obsolete]
    public static FuelScaleChargeResult GetFuelScaleChargeFromCO2Live(DateTime date, byte periodLength, int co2) {
      var client = new FuelScaleChargeClient();
      return client.GetFuelScaleChargeFromCO2(date, periodLength, co2);
    }

    /// <summary>Gets the fuel scale charge from the live HMRC website.</summary>
    /// <param name="date">The accounting period for which the scale charge should be retrieved.</param>
    /// <param name="period">The length of the VAT period.</param>
    /// <param name="co2">The CO2 emmissions (g/km) of the vehicle.</param>
    public static FuelScaleChargeResult GetFuelScaleChargeFromCO2Live(DateTime date, VatPeriod period, int co2) {
      var client = new FuelScaleChargeClient();
      return client.GetFuelScaleChargeFromCO2(date, period, co2);
    }

    /// <summary>
    /// Gets the fuel scale charge.
    /// Deprectated, used the <see cref="GetFuelScaleChargeFromCO2(DateTime, VatPeriod, int)"/> method instead.
    /// </summary>
    /// <param name="date">The accounting period for which the scale charge should be retrieved.</param>
    /// <param name="periodLength">The length of the VAT period in months (1, 3, 12).</param>
    /// <param name="co2">The CO2 emmissions (g/km) of the vehicle.</param>
    [Obsolete]
    public static FuelScaleChargeResult GetFuelScaleChargeFromCO2(DateTime date, byte periodLength, int co2) {
      if (!Enum.IsDefined(typeof(VatPeriod), periodLength))
        throw new ArgumentException($"{periodLength} is not valid.", nameof(periodLength));

      return GetFuelScaleChargeFromCO2(date, (VatPeriod)periodLength, co2);
    }

    /// <summary>Gets the fuel scale charge.</summary>
    /// <param name="date">The accounting period for which the scale charge should be retrieved.</param>
    /// <param name="period">The length of the VAT period.</param>
    /// <param name="co2">The CO2 emmissions (g/km) of the vehicle.</param>
    public static FuelScaleChargeResult GetFuelScaleChargeFromCO2(DateTime date, VatPeriod period, int co2) {
      var assembly = typeof(FuelScaleChargeResult).Assembly;
      var name = assembly.GetManifestResourceNames().Where(n => n.Contains(FuelScaleChargesResource)).First();

      FuelScaleChargeGroup[] values;
      using (var stream = assembly.GetManifestResourceStream(name)) {
        using (var reader = new StreamReader(stream)) {
          values = JsonConvert.DeserializeObject<FuelScaleChargeGroup[]>(reader.ReadToEnd());
        }
      }

      var group = values.Where(g => (g.From <= date) && (g.To >= date)).FirstOrDefault();
      if (group == null) {
        throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found for {date}.");
      }

      IEnumerable<FuelScaleChargeResult> list;
      switch (period) {
        case VatPeriod.Month:
          list = group.Monthly;
          break;

        case VatPeriod.Quarter:
          list = group.Quarterly;
          break;

        case VatPeriod.Annual:
          list = group.Annually;
          break;

        default:
          throw new Exception("Already checked for.");

      }

      var result = list.Where(x => co2 <= x.CO2Band).First();

      result.From = group.From;
      result.To = group.To;

      return result;
    }

    /// <summary>Retrieve VAT liabilities.</summary>
    /// <param name="request">The date range request.</param>
    public LiabilitiesResponse GetLiabilities(LiabilitiesRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<LiabilitiesResponse>(restRequest);
    }

    /// <summary>Retrieve VAT obligations.</summary>
    /// <param name="request">The obligations request.</param>
    public ObligationResponse GetObligations(ObligationsRequest request) {
      var restRequest = this.CreateRequest(request);

      var resp = this.ExecuteRequest<ObligationResponse>(restRequest);

      // HACK: The Api appears to return all obligations, regardless of status, filter them here
      if (request.Status != null) {
        resp.Value = resp.Value.Except(resp.Value.Where(x => x.Status != request.Status));
      }

      return resp;
    }

    /// <summary>Retrieve VAT payments.</summary>
    /// <param name="request">The date range request.</param>
    public PaymentsResponse GetPayments(PaymentsRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<PaymentsResponse>(restRequest);
    }

    /// <summary>Retrieve a submitted VAT return.</summary>
    /// <param name="request">The retrieval request.</param>
    public ReturnResponse GetReturn(ReturnRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<ReturnResponse>(restRequest);
    }

    /// <summary>Submit VAT return for period.</summary>
    /// <param name="request">The submission request.</param>
    public SubmitResponse SubmitReturn(SubmitRequest request) {
      var restRequest = this.CreateRequest(request);

      return this.ExecuteRequest<SubmitResponse>(restRequest);
    }
    #endregion
  }
}
