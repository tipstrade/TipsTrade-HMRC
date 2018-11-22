using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.Vat.Model;

namespace FuelScaleCharge {
  class Program {
    private const string DateFormat = "d MMMM yyyy";

    private const string Host = "http://www.gov.uk";
    private const string IndexUri = Host + "/government/publications/vat-road-fuel-scale-charges-table";

    /// <summary>The first supported date.</summary>
    private static readonly DateTime MinDate = new DateTime(2012, 5, 1);

    private const string OutputFile = "FuelScaleCharges.json";

    static async Task Main(string[] args) {
      CultureInfo.CurrentCulture = new CultureInfo("en-GB");

      await MainAsync();

      Console.WriteLine("Press any key to continue...");
      Console.Read();
    }

    static async Task MainAsync() {
      var links = await GetPages();
      var groups = new List<FuelScaleChargeGroup>();

      foreach (var uri in links) {
        var group = await GetValues(uri);
        if (group != null) {
          groups.Add(group);
        }
      }

      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(JsonConvert.SerializeObject(groups, Formatting.Indented));
      Console.ResetColor();
      Console.WriteLine();

      var compacted = JsonConvert.SerializeObject(groups);
      compacted = Regex.Replace(compacted, "\"(From|To)\":\"0001-01-01T00:00:00\",", "");

      Console.WriteLine();
      Console.WriteLine($"JSON written to {OutputFile}");
      using (var writer = new StreamWriter(OutputFile)) {
        writer.Write(compacted);
      }
    }

    private static Tuple<DateTime, DateTime> GetDates(HtmlDocument doc) {
      var months = string.Join("|", DateTimeFormatInfo.CurrentInfo.MonthNames.Where(m => !string.IsNullOrEmpty(m)).ToArray());

      var re = new Regex("((?<Day>[1-3]?[0-9]) (?<Month>" + months + ") (?<Year>2[0-9]{3}))");

      var title = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']")?.Attributes["content"]?.Value ?? "";
      var matches = re.Matches(title);

      return new Tuple<DateTime, DateTime>(
        DateTime.ParseExact(matches.First().Value, DateFormat, CultureInfo.CurrentCulture),
        DateTime.ParseExact(matches.Last().Value, DateFormat, CultureInfo.CurrentCulture)
        );
    }

    private static async Task<IEnumerable<string>> GetPages() {
      var doc = new HtmlDocument();
      using (var client = new HttpClient()) {
        using (var stream = await client.GetStreamAsync(IndexUri)) {
          doc.Load(stream);
        }
      }

      return doc.DocumentNode.SelectNodes("//a[@class='thumbnail']")
        .Select(n => Host + n.Attributes["href"].Value)
        .Distinct();
    }

    private static async Task<FuelScaleChargeGroup> GetValues(string uri) {
      Console.Write($"Fetching {uri} ... ");
      Console.WriteLine("done");

      var doc = new HtmlDocument();
      var req = WebRequest.Create(uri);
      req.Headers.Add("Referer", IndexUri);
      req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");

      using (var resp = await req.GetResponseAsync()) {
        using (var stream = resp.GetResponseStream()) {
          doc.Load(stream);
        }
      }

      var dates = GetDates(doc);
      Console.WriteLine($"\t{dates.Item1} -> {dates.Item2}");

      if (dates.Item1 < MinDate) {
        Console.WriteLine($"\tNot supported for dates before {MinDate}.");
        return null;
      }

      var tables = doc.DocumentNode.SelectNodes("//table");
      var annual = tables[tables.Count - 3];
      var quarterly = tables[tables.Count - 2];
      var monthly = tables[tables.Count -1];

      var group = new FuelScaleChargeGroup() {
        From = dates.Item1,
        To = dates.Item2
      };

      group.Annually.AddRange(GetValues(annual));
      group.Quarterly.AddRange(GetValues(quarterly));
      group.Monthly.AddRange(GetValues(monthly));

      ValidateGroup(group);

      return group;
    }

    private static IEnumerable<FuelScaleChargeResult> GetValues(HtmlNode table) {
      var intExpression = new Regex("([0-9]+)");

      var resp = new List<FuelScaleChargeResult>();

      bool isStarted = false;
      foreach (var row in table.SelectNodes(".//tbody/tr")) {
        if (!isStarted && !row.InnerText.Contains("or less")) {
          continue;
        }
        isStarted = true;

        var cells = row.SelectNodes(".//td");

        try {
          var scaleCharge = new FuelScaleChargeResult() {
            CO2Band = int.Parse(intExpression.Match(cells[0].InnerText).Value, NumberStyles.AllowThousands, CultureInfo.CurrentCulture),
            Vat = decimal.Parse(cells[2].InnerText, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.CurrentCulture),
            Nett = decimal.Parse(cells[3].InnerText, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.CurrentCulture)
          };

          // CO2Band is actually the upper bound, but these pages use round-down to next 5g/km
          scaleCharge.CO2Band += 4;

          resp.Add(scaleCharge);
        } catch (Exception) {
          throw;
        }
      }

      resp.Last().CO2Band = int.MaxValue;

      return resp;
    }

    private static void ValidateGroup(FuelScaleChargeGroup group) {
      var errors = new List<string>();

      if (group.Annually.Count == 0) {
        errors.Add($"{nameof(group.Annually)} cannot be empty.");
      }
      if (group.Quarterly.Count == 0) {
        errors.Add($"{nameof(group.Quarterly)} cannot be empty.");
      }
      if (group.Monthly.Count == 0) {
        errors.Add($"{nameof(group.Monthly)} cannot be empty.");
      }
      if (group.Quarterly.Count != group.Annually.Count) {
        errors.Add($"{nameof(group.Quarterly)} is not the same length as {nameof(group.Annually)}");
      }
      if (group.Monthly.Count != group.Annually.Count) {
        errors.Add($"{nameof(group.Monthly)} is not the same length as {nameof(group.Annually)}");
      }

      ValidateList(group.Annually, errors);
      ValidateList(group.Quarterly, errors);
      ValidateList(group.Monthly, errors);

      if (errors.Count == 0) {
        return;
      }

      Console.ForegroundColor = ConsoleColor.Red;
      foreach (var item in errors) {
        Console.WriteLine(item);
      }
      Console.WriteLine(JsonConvert.SerializeObject(group, Formatting.Indented));
      Console.ResetColor();
      Console.Error.WriteLine();

      throw new Exception("Validation failed");
    }

    private static void ValidateList(IEnumerable<FuelScaleChargeResult> fuelScaleCharges, List<string> errors) {
      decimal vatRate = fuelScaleCharges.First().VatRate;
      FuelScaleChargeResult last = null;

      foreach (var item in fuelScaleCharges) {
        if (last != null) {
          if (last.CO2Band >= item.CO2Band) {
            errors.Add($"{nameof(item.CO2Band)} has decreased from last item.");
          }
          if (last.Nett >= item.Nett) {
            errors.Add($"{nameof(item.Nett)} has decreased from last item.");
          }
          if (last.Vat >= item.Vat) {
            errors.Add($"{nameof(item.Vat)} has decreased from last item.");
          }
        }
        if (vatRate != item.VatRate) {
          errors.Add($"VAT Rate has changed from {vatRate} to {item.VatRate}");
        }
        if (item.Nett == 0) {
          errors.Add($"{nameof(item.Nett)} cannot be 0");
        }
        if (item.Vat == 0) {
          errors.Add($"{nameof(item.Vat)} cannot be 0");
        }
        if (item.CO2Band == 0) {
          errors.Add($"{nameof(item.CO2Band)} cannot be 0");
        }

        last = item;
      }
    }
  }
}
