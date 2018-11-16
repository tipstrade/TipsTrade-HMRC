using System;

namespace TipsTrade.HMRC.Api {
  /// <summary>The factory used for creating IApi objects.</summary>
  internal class ApiFactory<T> where T : class, IApi, IClient {
    internal static T Create(Client client) {
      T api = Activator.CreateInstance<T>();
      api.Client = client;
      return api;
    }
  }
}
