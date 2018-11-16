using System;
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

    /// <summary>Creates an ICreateTestUserRequest object with all possible service names.</summary>
    public static T CreateTestUserFull() {
      var request = CreateTestUser();

      var serviceNames = typeof(T)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.GetCustomAttribute<ServiceNameAttribute>() != null)
        .Select(f => (string)f.GetValue(null));

      request.ServiceNames.AddRange(serviceNames);

      return request;
    }
  }
}
