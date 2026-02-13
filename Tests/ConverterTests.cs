using System;
using System.Collections.Generic;
using Xunit;
using Nsft = Newtonsoft.Json;
using Stj = System.Text.Json;

namespace TipsTrade.HMRC.Tests {
  public class ConverterTests {
    [Fact]
    public void DateOnlySerializationNewtonsoft() {
      var expectedDate = DateTime.Now;
      var expectedString = $"\"{expectedDate.ToString("yyyy-MM-dd")}\"";

      var settings = new Nsft.JsonSerializerSettings {
        Converters = new List<Nsft.JsonConverter>() { new Api.Model.Converters.NewtonsoftDateOnlyConverter() }
      };

      var actualString = Nsft.JsonConvert.SerializeObject(expectedDate, settings);

      Assert.Equal(expectedString, actualString);

      var actualDate = Nsft.JsonConvert.DeserializeObject<DateTime>(actualString, settings);

      Assert.Equal(expectedDate.Date, actualDate);
    }

    [Fact]
    public void DateOnlySerializationNewtonsoft_Throws() {
      var settings = new Nsft.JsonSerializerSettings {
        Converters = new List<Nsft.JsonConverter>() { new Api.Model.Converters.NewtonsoftDateOnlyConverter() }
      };

      var ex1 = Assert.Throws<Nsft.JsonSerializationException>(() => { Nsft.JsonConvert.DeserializeObject<DateTime>("\"invalid-date\"", settings); });
      var ex2 = Assert.Throws<Nsft.JsonSerializationException>(() => { Nsft.JsonConvert.DeserializeObject<DateTime>("\"2026-02-12T11:39:22.878Z\"", settings); });
      var ex3 = Assert.Throws<Nsft.JsonSerializationException>(() => { Nsft.JsonConvert.DeserializeObject<DateTime>("0", settings); });
    }

    [Fact]
    public void DateOnlySerializationSystemTextJson() {
      var expectedDate = DateTime.Now;
      var expectedString = $"\"{expectedDate.ToString("yyyy-MM-dd")}\"";

      var settings = new Stj.JsonSerializerOptions();
      settings.Converters.Add(new Api.Model.Converters.StjDateOnlyConverter());

      var actualString = Stj.JsonSerializer.Serialize(expectedDate, settings);

      Assert.Equal(expectedString, actualString);

      var actualDate = Stj.JsonSerializer.Deserialize<DateTime>(actualString, settings);

      Assert.Equal(expectedDate.Date, actualDate);
    }

    [Fact]
    public void DateOnlySerializationSystemTextJson_Throws() {
      var settings = new Stj.JsonSerializerOptions();
      settings.Converters.Add(new Api.Model.Converters.StjDateOnlyConverter());

      var ex1 = Assert.Throws<Stj.JsonException>(() => { Stj.JsonSerializer.Deserialize<DateTime>("\"invalid-date\"", settings); });
      var ex2 = Assert.Throws<Stj.JsonException>(() => { Stj.JsonSerializer.Deserialize<DateTime>("\"2026-02-12T11:39:22.878Z\"", settings); });
      var ex3 = Assert.Throws<Stj.JsonException>(() => { Stj.JsonSerializer.Deserialize<DateTime>("0", settings); });
    }
  }
}
