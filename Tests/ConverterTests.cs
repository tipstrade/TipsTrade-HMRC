using System;
using System.Collections.Generic;
using NUnit.Framework;
using Nsft = Newtonsoft.Json;
using Stj = System.Text.Json;

namespace TipsTrade.HMRC.Tests {
  public class ConverterTests {
    [Test]
    public void DateOnlySerializationNewtonsoft() {
      var expectedDate = DateTime.Now;
      var expectedString = $"\"{expectedDate.ToString("yyyy-MM-dd")}\"";

      var settings = new Nsft.JsonSerializerSettings {
        Converters = new List<Nsft.JsonConverter>() { new Api.Model.Converters.NewtonsoftDateOnlyConverter() }
      };

      var actualString = Nsft.JsonConvert.SerializeObject(expectedDate, settings);
      Assert.That(actualString, Is.EqualTo(expectedString));

      var actualDate = Nsft.JsonConvert.DeserializeObject<DateTime>(actualString, settings);
      Assert.That(actualDate, Is.EqualTo(expectedDate.Date));
    }

    [Test]
    public void DateOnlySerializationNewtonsoft_Attributes() {
      var expectedObj = new TestDateClass {
        Date = new DateTime(2024, 1, 1).Date,
        OptionalDate = new DateTime(2024, 1, 1).Date
      };

      var jsonNulls = Nsft.JsonConvert.SerializeObject(new TestDateClass { });
      var actualJsonNull = Nsft.JsonConvert.DeserializeObject<TestDateClass>(jsonNulls);
      Assert.That(jsonNulls, Is.EqualTo("{\"Date\":\"0001-01-01\",\"OptionalDate\":null}"));
      Assert.That(actualJsonNull.Date, Is.EqualTo(default(DateTime)));
      Assert.That(actualJsonNull.OptionalDate, Is.Null);

      var jsonNonNull = Nsft.JsonConvert.SerializeObject(expectedObj);
      var actualJsonNonNull = Nsft.JsonConvert.DeserializeObject<TestDateClass>(jsonNonNull);
      Assert.That(jsonNonNull, Is.EqualTo("{\"Date\":\"2024-01-01\",\"OptionalDate\":\"2024-01-01\"}"));
      Assert.That(actualJsonNonNull.Date, Is.EqualTo(expectedObj.Date));
      Assert.That(actualJsonNonNull.OptionalDate, Is.EqualTo(expectedObj.OptionalDate));
    }

    [Test]
    public void DateOnlySerializationNewtonsoft_Nullable() {
      DateTime? expectedDate = null;
      var expectedString = "null";

      var settings = new Nsft.JsonSerializerSettings {
        Converters = new List<Nsft.JsonConverter>() { new Api.Model.Converters.NewtonsoftDateOnlyConverter() }
      };

      var actualString = Nsft.JsonConvert.SerializeObject(expectedDate, settings);
      Assert.That(actualString, Is.EqualTo(expectedString));

      var actualDate = Nsft.JsonConvert.DeserializeObject<DateTime?>(actualString, settings);
      Assert.That(actualDate, Is.EqualTo(expectedDate));

      var defaultDate = Nsft.JsonConvert.DeserializeObject<DateTime>("null", settings);
      Assert.That(defaultDate, Is.EqualTo(default(DateTime)));
    }

    [Test]
    public void DateOnlySerializationNewtonsoft_Throws() {
      var settings = new Nsft.JsonSerializerSettings {
        Converters = new List<Nsft.JsonConverter>() { new Api.Model.Converters.NewtonsoftDateOnlyConverter() }
      };

      Assert.That((Action)(() => Nsft.JsonConvert.DeserializeObject<DateTime>("\"invalid-date\"", settings)), Throws.InstanceOf<Nsft.JsonSerializationException>());
      Assert.That((Action)(() => Nsft.JsonConvert.DeserializeObject<DateTime>("\"2026-02-12T11:39:22.878Z\"", settings)), Throws.InstanceOf<Nsft.JsonSerializationException>());
      Assert.That((Action)(() => Nsft.JsonConvert.DeserializeObject<DateTime>("0", settings)), Throws.InstanceOf<Nsft.JsonSerializationException>());
    }

    [Test]
    public void DateOnlySerializationSystemTextJson() {
      var expectedDate = DateTime.Now;
      var expectedString = $"\"{expectedDate.ToString("yyyy-MM-dd")}\"";

      var settings = new Stj.JsonSerializerOptions();
      settings.Converters.Add(new Api.Model.Converters.StjDateOnlyConverter());

      var actualString = Stj.JsonSerializer.Serialize(expectedDate, settings);
      Assert.That(actualString, Is.EqualTo(expectedString));

      var actualDate = Stj.JsonSerializer.Deserialize<DateTime>(actualString, settings);
      Assert.That(actualDate, Is.EqualTo(expectedDate.Date));
    }

    [Test]
    public void DateOnlySerializationSystemTextJson_Attributes() {
      var expectedObj = new TestDateClass {
        Date = new DateTime(2024, 1, 1).Date,
        OptionalDate = new DateTime(2024, 1, 1).Date
      };

      var jsonNulls = Stj.JsonSerializer.Serialize(new TestDateClass { });
      var actualJsonNull = Stj.JsonSerializer.Deserialize<TestDateClass>(jsonNulls);
      Assert.That(jsonNulls, Is.EqualTo("{\"Date\":\"0001-01-01\",\"OptionalDate\":null}"));
      Assert.That(actualJsonNull.Date, Is.EqualTo(default(DateTime)));
      Assert.That(actualJsonNull.OptionalDate, Is.Null);

      var jsonNonNull = Stj.JsonSerializer.Serialize(expectedObj);
      var actualJsonNonNull = Stj.JsonSerializer.Deserialize<TestDateClass>(jsonNonNull);
      Assert.That(jsonNonNull, Is.EqualTo("{\"Date\":\"2024-01-01\",\"OptionalDate\":\"2024-01-01\"}"));
      Assert.That(actualJsonNonNull.Date, Is.EqualTo(expectedObj.Date));
      Assert.That(actualJsonNonNull.OptionalDate, Is.EqualTo(expectedObj.OptionalDate));
    }

    [Test]
    public void DateOnlySerializationSystemTextJson_Nullable() {
      DateTime? expectedDate = null;
      var expectedString = "null";

      var settings = new Stj.JsonSerializerOptions();
      settings.Converters.Add(new Api.Model.Converters.StjDateOnlyConverter());

      var actualString = Stj.JsonSerializer.Serialize(expectedDate, settings);
      Assert.That(actualString, Is.EqualTo(expectedString));

      var actualDate = Stj.JsonSerializer.Deserialize<DateTime?>(actualString, settings);
      Assert.That(actualDate, Is.EqualTo(expectedDate));
    }

    [Test]
    public void DateOnlySerializationSystemTextJson_Throws() {
      var settings = new Stj.JsonSerializerOptions();
      settings.Converters.Add(new Api.Model.Converters.StjDateOnlyConverter());

      Assert.That((Action)(() => Stj.JsonSerializer.Deserialize<DateTime>("\"invalid-date\"", settings)), Throws.InstanceOf<Stj.JsonException>());
      Assert.That((Action)(() => Stj.JsonSerializer.Deserialize<DateTime>("\"2026-02-12T11:39:22.878Z\"", settings)), Throws.InstanceOf<Stj.JsonException>());
      Assert.That((Action)(() => Stj.JsonSerializer.Deserialize<DateTime>("0", settings)), Throws.InstanceOf<Stj.JsonException>());
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
