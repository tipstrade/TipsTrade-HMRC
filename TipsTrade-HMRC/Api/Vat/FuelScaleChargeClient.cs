using HtmlAgilityPack;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  internal class FuelScaleChargeClient {
    private const string BaseUri = "https://www.gov.uk";

    private const string InitialUri = "fuel-scale-charge/y";

    private RestClient Client { get; } = new RestClient(BaseUri);

    private string GetBand(string dates, string month, int co2) {
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

        if ((co2 >= lower) && (co2 <= upper)) {
          return value;
        }
      }

      throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found for {co2}.");
    }

    internal FuelScaleChargeResult GetFuelScaleChargeFromCO2(DateTime date, byte periodLength, int co2) {
      string month = GetMonth(periodLength);

      var dateString = GetDates().Where(d => (date >= d.From) && (date <= d.To))?.FirstOrDefault().Value;
      if (dateString == null) {
        throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found for {date}.");
      }

      var band = GetBand(dateString, month, co2);

      return GetValues(dateString, month, band);
    }

    private IEnumerable<DateRange> GetDates() {
      var request = new RestRequest(InitialUri);
      var response = Client.Execute(request);

      var doc = new HtmlDocument();
      doc.LoadHtml(response.Content);

      var dates = doc.DocumentNode.SelectNodes("//input[@type='radio']");
      if (dates == null) {
        throw new InvalidOperationException("No valid Fuel Scale Charge date ranges could be found.");
      }

      return dates.Select(d => {
        return new DateRange(d.Attributes["value"].Value);
      });
    }

    private string GetMonth(byte periodLength) {
      switch (periodLength) {
        case 1:
          return "1-month-monthly";

        case 3:
          return "3-months-quarterly";

        case 12:
          return "12-months-annual";

        default:
          throw new ArgumentException($"{periodLength} is not valid.", nameof(periodLength));

      }
    }

    private FuelScaleChargeResult GetValues(string dates, string month, string band) {
      var request = new RestRequest($"{InitialUri}/{dates}/{month}/{band}");
      var response = Client.Execute(request);

      var doc = new HtmlDocument();
      doc.LoadHtml(response.Content);

      var result = doc.DocumentNode.SelectSingleNode("//div[@class='result-info]/p");
      if (result == null) {
        throw new InvalidOperationException($"No {nameof(FuelScaleChargeResult)} data could be found.");
      }

      var matches = Regex.Matches(result.InnerText, "([0-9](+\\.[0-9]+)?)");
      return new FuelScaleChargeResult() {
        Nett = decimal.Parse(matches[0].Value),
        Vat = decimal.Parse(matches[1].Value)
      };

      return null;
    }

    #region Inner classes
    private class Band<T> {
      internal T From { get; set; }

      internal T To { get; set; }

      internal string Value { get; set}
    }

    private class DateRange {
      private const string DateFormat = "d-MMMM-yyyy";

      internal DateTime From { get; set; }

      internal DateTime To { get; set; }

      internal string Value { get; set; }

      internal DateRange(string value) {
        var delimiter = value.IndexOf("-to-"); // 452 doesn't support string.Split(string)
        Value = value;
        From = DateTime.ParseExact(value.Substring(0, delimiter), DateFormat, CultureInfo.CurrentCulture);
        To = DateTime.ParseExact(value.Substring(delimiter + 4), DateFormat, CultureInfo.CurrentCulture);
      }
    }
    #endregion
  }
}
