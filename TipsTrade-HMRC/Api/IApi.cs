namespace TipsTrade.HMRC.Api {
  /// <summary>The description of a HMRC API.</summary>
  public interface IApi {
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
