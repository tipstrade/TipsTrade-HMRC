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
    public void DateOnlySerializationNewtonsoft_Attributes() {
      var expectedObj = new TestDateClass {
        Date = new DateTime(2024, 1, 1).Date,
        OptionalDate = new DateTime(2024, 1, 1).Date
      };

      var jsonNulls = Nsft.JsonConvert.SerializeObject(new TestDateClass { });
      var actualJsonNull = Nsft.JsonConvert.DeserializeObject<TestDateClass>(jsonNulls);
      Assert.Equal("{\"Date\":\"0001-01-01\",\"OptionalDate\":null}", jsonNulls);
      Assert.Equal(default(DateTime), actualJsonNull.Date);
      Assert.Null(actualJsonNull.OptionalDate);

      var jsonNonNull = Nsft.JsonConvert.SerializeObject(expectedObj);
      var actualJsonNonNull = Nsft.JsonConvert.DeserializeObject<TestDateClass>(jsonNonNull);
      Assert.Equal("{\"Date\":\"2024-01-01\",\"OptionalDate\":\"2024-01-01\"}", jsonNonNull);
      Assert.Equal(expectedObj.Date, actualJsonNonNull.Date);
      Assert.Equal(expectedObj.OptionalDate, actualJsonNonNull.OptionalDate);
    }

    [Fact]
    public void DateOnlySerializationNewtonsoft_Nullable() {
      DateTime? expectedDate = null;
      var expectedString = "null";

      var settings = new Nsft.JsonSerializerSettings {
        Converters = new List<Nsft.JsonConverter>() { new Api.Model.Converters.NewtonsoftDateOnlyConverter() }
      };

      var actualString = Nsft.JsonConvert.SerializeObject(expectedDate, settings);
      Assert.Equal(expectedString, actualString);

      var actualDate = Nsft.JsonConvert.DeserializeObject<DateTime?>(actualString, settings);
      Assert.Equal(expectedDate, actualDate);

      var defaultDate = Nsft.JsonConvert.DeserializeObject<DateTime>("null", settings);
      Assert.Equal(default, defaultDate);
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
    public void DateOnlySerializationSystemTextJson_Attributes() {
      var expectedObj = new TestDateClass {
        Date = new DateTime(2024, 1, 1).Date,
        OptionalDate = new DateTime(2024, 1, 1).Date
      };

      var jsonNulls = Stj.JsonSerializer.Serialize(new TestDateClass { });
      var actualJsonNull = Stj.JsonSerializer.Deserialize<TestDateClass>(jsonNulls);
      Assert.Equal("{\"Date\":\"0001-01-01\",\"OptionalDate\":null}", jsonNulls);
      Assert.Equal(default, actualJsonNull.Date);
      Assert.Null(actualJsonNull.OptionalDate);

      var jsonNonNull = Stj.JsonSerializer.Serialize(expectedObj);
      var actualJsonNonNull = Stj.JsonSerializer.Deserialize<TestDateClass>(jsonNonNull);
      Assert.Equal("{\"Date\":\"2024-01-01\",\"OptionalDate\":\"2024-01-01\"}", jsonNonNull);
      Assert.Equal(expectedObj.Date, actualJsonNonNull.Date);
      Assert.Equal(expectedObj.OptionalDate, actualJsonNonNull.OptionalDate);
    }

    [Fact]
    public void DateOnlySerializationSystemTextJson_Nullable() {
      DateTime? expectedDate = null;
      var expectedString = "null";

      var settings = new Stj.JsonSerializerOptions();
      settings.Converters.Add(new Api.Model.Converters.StjDateOnlyConverter());

      var actualString = Stj.JsonSerializer.Serialize(expectedDate, settings);
      Assert.Equal(expectedString, actualString);

      var actualDate = Stj.JsonSerializer.Deserialize<DateTime?>(actualString, settings);
      Assert.Equal(expectedDate, actualDate);
    }

    [Fact]
    public void DateOnlySerializationSystemTextJson_Throws() {
      var settings = new Stj.JsonSerializerOptions();
      settings.Converters.Add(new Api.Model.Converters.StjDateOnlyConverter());

      var ex1 = Assert.Throws<Stj.JsonException>(() => { Stj.JsonSerializer.Deserialize<DateTime>("\"invalid-date\"", settings); });
      var ex2 = Assert.Throws<Stj.JsonException>(() => { Stj.JsonSerializer.Deserialize<DateTime>("\"2026-02-12T11:39:22.878Z\"", settings); });
      var ex3 = Assert.Throws<Stj.JsonException>(() => { Stj.JsonSerializer.Deserialize<DateTime>("0", settings); });
    }

    #region Inner classes
    public class TestDateClass {
      [Nsft.JsonConverter(typeof(Api.Model.Converters.NewtonsoftDateOnlyConverter))]
      [Stj.Serialization.JsonConverter(typeof(Api.Model.Converters.StjDateOnlyConverter))]
      public DateTime Date { get; set; }

      [Nsft.JsonConverter(typeof(Api.Model.Converters.NewtonsoftDateOnlyConverter))]
      [Stj.Serialization.JsonConverter(typeof(Api.Model.Converters.StjDateOnlyConverter))]
      public DateTime? OptionalDate { get; set; }
    }
    #endregion
  }
}
