using HtmlAgilityPack;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>
  /// Represents a client that fetches VAT Fuel Scale Charge from the HMRC online calculator.
  /// See: <see cref="!:https://www.gov.uk/fuel-scale-charge"></see>
  /// </summary>
  public class FuelScaleChargeClient {
    #region Fields
    private const string BaseUri = "https://www.gov.uk";

    private const string InitialUri = "fuel-scale-charge/y";
    #endregion

    #region Properties
    private RestClient Client { get; } = new RestClient(BaseUri);
    #endregion

    #region Methods
    private IEnumerable<Band<int>> GetBands(string dates, string month) {
      var request = new RestRequest($"{InitialUri}/{dates}/{month}");
      var response = Client.Execute(request);

      var doc = new HtmlDocument();
      doc.LoadHtml(response.Content);

      var bands = doc.DocumentNode.SelectNodes("//input[@type='radio']");
      if (bands == null) {
        throw new InvalidOperationException("No valid Fuel Scale Charge CO2 bands could be found.");
      }

      var numberRegex = new Regex("([0-9]+)");
      bool isFirst = true;
      var list = new List<Band<int>>();
      foreach (var item in bands) {
        var value = item.Attributes["value"].Value;
        var matches = numberRegex.Matches(value);

        int lower, upper;
        if (isFirst) {
          lower = 0;
          upper = int.Parse(matches[0].Value);
        } else {
          lower = int.Parse(matches[0].Value);
          upper = matches.Count == 1 ? int.MaxValue : int.Parse(matches[1].Value);
        }

        list.Add(new Band<int>() {
          Value = value,
          From = lower,
          To = upper
        });

        isFirst = false;
      }

      return list;
    }

    /// <summary>
    /// Gets the Fuel Scale Charge.
    /// Deprectated, used the <see cref="GetFuelScaleChargeFromCO2(DateTime, VatPeriod, int)"/> method instead.
    /// </summary>
    /// <param name="date">The accounting period for which the scale charge should be retrieved.</param>
    /// <param name="periodLength">The length of the VAT period in months (1, 3, 12).</param>
    /// <param name="co2">The CO2 emmissions (g/km) of the vehicle.</param>
    [Obsolete]
    public FuelScaleChargeResult GetFuelScaleChargeFromCO2(DateTime date, byte periodLength, int co2) {
      if (!Enum.IsDefined(typeof(VatPeriod), periodLength))
        throw new ArgumentException($"{periodLength} is not valid.", nameof(periodLength));

      return GetFuelScaleChargeFromCO2(date, (VatPeriod)periodLength, co2);
    }

    /// <summary>Gets the Fuel Scale Charge.</summary>
    /// <param name="date">The accounting period for which the scale charge should be retrieved.</param>
    /// <param name="period">The length of the VAT period.</param>
    /// <param name="co2">The CO2 emmissions (g/km) of the vehicle.</param>
    public FuelScaleChargeResult GetFuelScaleChargeFromCO2(DateTime date, VatPeriod period, int co2) {
      string months = GetMonth(period);

      var dates = GetDates().Where(d => (date >= d.From) && (date <= d.To))?.FirstOrDefault();
      if (dates == null) {
        throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found for {date}.");
      }

      var bands = GetBands(dates.Value, months).Where(b => (co2 >= b.From) && (co2 <= b.To))?.FirstOrDefault();
      if (bands == null) {
        throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found for {co2}.");
      }

      return GetValues(dates, months, bands);
    }

    private IEnumerable<Band<DateTime>> GetDates() {
      var request = new RestRequest(InitialUri);
      var response = Client.Execute(request);

      var doc = new HtmlDocument();
      doc.LoadHtml(response.Content);

      var dates = doc.DocumentNode.SelectNodes("//input[@type='radio']");
      if (dates == null) {
        throw new InvalidOperationException("No valid Fuel Scale Charge date ranges could be found.");
      }

      const string DateFormat = "d-MMMM-yyyy";

      return dates.Select(d => {
        var value = d.Attributes["value"].Value;
        var delimiter = value.IndexOf("-to-"); // 452 doesn't support string.Split(string)
        return new Band<DateTime>() {
          Value = value,
          From = DateTime.ParseExact(value.Substring(0, delimiter), DateFormat, CultureInfo.CurrentCulture),
          To = DateTime.ParseExact(value.Substring(delimiter + 4), DateFormat, CultureInfo.CurrentCulture)
        };
      });
    }

    private string GetMonth(VatPeriod periodLength) {
      switch (periodLength) {
        case  VatPeriod.Month:
          return "1-month-monthly";

        case  VatPeriod.Quarter :
          return "3-months-quarterly";

        case VatPeriod.Annual:
          return "12-months-annual";

        default:
          throw new ArgumentException($"{periodLength} is not valid.", nameof(periodLength));

      }
    }

    private FuelScaleChargeResult GetValues(Band<DateTime> dates, string month, Band<int> band) {
      var request = new RestRequest($"{InitialUri}/{dates.Value}/{month}/{band.Value}");
      var response = Client.Execute(request);

      var doc = new HtmlDocument();
      doc.LoadHtml(response.Content);

      var result = doc.DocumentNode.SelectSingleNode("//div[@class='result-info']/p");
      if (result == null) {
        throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found.");
      }

      var matches = Regex.Matches(result.InnerText, "([0-9]+(\\.[0-9]+)?)");
      if (matches.Count == 0) {
        throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found.");
      }

      return new FuelScaleChargeResult() {
        CO2Band = band.To,
        From = dates.From,
        To = dates.To,
        Nett = decimal.Parse(matches[0].Value),
        Vat = decimal.Parse(matches[1].Value)
      };
    }
    #endregion

    #region Inner classes
    private class Band<T> {
      internal T From { get; set; }

      internal T To { get; set; }

      internal string Value { get; set; }
    }
    #endregion
  }
}
