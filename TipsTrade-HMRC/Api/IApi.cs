namespace TipsTrade.HMRC.Api {
  /// <summary>The description of a HMRC API.</summary>
  public interface IApi {
    /*
    // This code is used to parse the list of errors from the developer console of a documentaion page
    var errors = {};
    $("[data-heading='Code']").each(function(index, item) {
      item = $(item);
      var code = item.find("p").text();
      var description = item.parent().find("[data-heading='Error Scenario'] p").text();

      var list = errors[code] || [];
      errors[code] = list;

      var found = false;
      for (var i = 0; i < list.length; i++) {
        if (list[i] == description) {
          found = true;
          break;
        }
      }

      if (!found) {
        list.push(description);
      }

    });

    var csharp = "";
    Object.keys(errors).sort().forEach(function(code) {
      csharp += "///<summary>\n///" + errors[code].join("\n///") + "\n///</summary>\n";
      csharp += "public const string ERROR_" + code + " = \"" + code + "\";\n\n";
    });

    errors;
    copy(csharp);
     */

    /// <summary>The description of the API.</summary>
    string Description { get; }

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    bool IsStable { get; }

    /// <summary>The relative location of the API.</summary>
    string Location { get; }

    /// <summary>The name of the API.</summary>
    string Name { get; }

    /// <summary>The version of the API that the client should target.</summary>
    string Version { get; }
  }
}
