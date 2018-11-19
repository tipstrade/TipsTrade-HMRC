using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TipsTrade.HMRC.Api.CreateTestUser.Model;

namespace TipsTrade.HMRC.Api.CreateTestUser {
  /// <summary>The factory used for creating ICreateTestUserRequest objects.</summary>
  public class CreateTestUserFactory<T> where T : class, ICreateTestUserRequest {
    /// <summary>Creates an empty ICreateTestUserRequest object.</summary>
    public static T CreateTestUser() {
      return Activator.CreateInstance<T>();
    }

    /// <summary>Creates an ICreateTestUserRequest object with service names matching the specified predicate.</summary>
    public static T CreateTestUser(Func<string, bool> predicate) {
      var request = CreateTestUser();

      request.ServiceNames.AddRange(GetServiceNames<T>().Where(predicate));

      return request;
    }

    /// <summary>Creates an ICreateTestUserRequest object with all possible service names.</summary>
    public static T CreateTestUserFull() {
      return CreateTestUser(s => true);
    }

    private static IEnumerable<string> GetServiceNames<T>() {
      return typeof(T)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.GetCustomAttribute<ServiceNameAttribute>() != null)
        .Select(f => (string)f.GetValue(null));
    }
  }
}
