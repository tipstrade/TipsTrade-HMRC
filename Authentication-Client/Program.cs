using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Tests.Authentication_Client {
  class Program {
    #region Enumerations
    private enum MainMenu {
      RefreshToken = 1,
      CreateUser = 2
    }
    #endregion

    #region Properties
    private static IConfiguration Configuration { get; set; }

    private static string ClientId => Configuration.GetSection(Environment)["ClientID"];

    private static string ClientSecret => Configuration.GetSection(Environment)["ClientSecret"];

    private static string Environment => IsSandbox ? "Sandbox" : "Production";

    private static bool IsSandbox { get; set; } = true;

    protected static string ServerToken => Configuration.GetSection(Environment)["ServerToken"];
    #endregion

    #region Entry point
    static void Main(string[] args) {
      if (args.Any(s => Flags.UseProduction.Equals(s))) {
        IsSandbox = false;
        args = args.Except(args.Where(s => Flags.UseProduction.Equals(s))).ToArray();
      }

      var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>()
        ;

      Configuration = builder.Build();

      try {
        var client = GetClient();

        switch (ShowMainMenu()) {
          case MainMenu.RefreshToken:
            RefreshToken(client);
            break;

          case MainMenu.CreateUser:
            Authenticate(client);
            break;

        }

      } catch (Exception ex) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine("");
        Console.Error.WriteLine($"An {ex.GetType()} was thrown: {ex.Message}");
        Console.Error.WriteLine(ex.StackTrace);
        Console.ResetColor();
        Console.Write("");
      }


      Console.WriteLine();
      Console.Write("Press any key to continue...");
      Console.Read();
    }
    #endregion

    #region Methods
    private static void Authenticate(Client client) {
      var userType = GetUserType(client);
      var user = CreateUser(client, userType);
      var tokens = GetAuthCode(client, user);

      Console.WriteLine();
      Console.WriteLine("Copy these details into appsettings.tokens.json for testing:");

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(JsonConvert.SerializeObject(new {
        User = user,
        Tokens = tokens
      }, Formatting.Indented));
      Console.ResetColor();
      Console.Write("");
    }

    private static UserResultBase CreateUser(Client client, Type userType) {
      var request = Activator.CreateInstance(userType) as ICreateTestUserRequest;
      request.ServiceNames.AddRange(request.GetServiceNames());

      Console.WriteLine();
      Console.Write($"Executing CreateUser({userType})...");
      var resp = client.CreateTestUser.CreateUser(request);
      Console.WriteLine(" done");

      return resp;
    }

    private static TokenResponse GetAuthCode(Client client, UserResultBase user) {
      var state = $"{Guid.NewGuid()}";
      var scopes = Scopes.GetScopes();
      var redirectUrl = Configuration["RedirectUrl"];
      var url = client.GetAuthorizatoinEndpoint(state, redirectUrl, scopes);

      Console.WriteLine();
      Console.WriteLine("Navigate to the link below, and login using the following credentials:");
      Console.WriteLine($"\t{url}");
      Console.WriteLine($"\tID:       {user.UserId}");
      Console.WriteLine($"\tPassword: {user.Password}");

      Console.WriteLine();
      Console.WriteLine($"Paste in the '{redirectUrl}' address that you were redirected to:");
      var redirectedTo = Console.ReadLine();

      Console.WriteLine();
      Console.Write("Validating...");
      var resp = client.HandleEndpointResult(redirectedTo, state);
      Console.WriteLine(" done");

      return resp;
    }

    private static Type GetUserType(Client client) {
      var types = client.GetType().Assembly.GetTypes().Where(t => {
        return (t != typeof(ICreateTestUserRequest)) && typeof(ICreateTestUserRequest).IsAssignableFrom(t);
      }).OrderBy(t => t.Name).ToArray();

      for (int i = 0; i < types.Length; i++) {
        Console.WriteLine($"{i + 1}: {types[i].Name}");
      }
      Console.Write("Enter the type of the user to create: ");
      string resp = Console.ReadLine();

      if (int.TryParse(resp, out int id)) {
        id--;
        if ((id >= 0) && (id < types.Length)) {
          return types[id];
        }
      }

      throw new Exception($"{resp} is not a valid user type.");
    }

    private static Client GetClient() => new Client(ClientId, ClientSecret, ServerToken, IsSandbox);

    private static void RefreshToken(Client client) {
      Console.WriteLine();
      Console.Write("Enter the refresh token: ");
      var refreshToken = Console.ReadLine();
      var tokens = client.RefreshAccessToken(refreshToken);

      Console.WriteLine();
      Console.WriteLine("Copy these details into appsettings.tokens.json for testing:");

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(JsonConvert.SerializeObject(new {
        Tokens = tokens
      }, Formatting.Indented));
      Console.ResetColor();
      Console.Write("");
    }

    private static MainMenu ShowMainMenu() {
      var items = Enum.GetValues(typeof(MainMenu)).Cast<MainMenu>();

      Console.WriteLine("Main menu");
      foreach (var item in items) {
        Console.WriteLine($"{item:d}:\t{item}");
      }
      Console.Write("Enter which function to run: ");
      var resp = Console.ReadLine();

      if (int.TryParse(resp, out int id)) {
        return (MainMenu)id;
      }

      throw new Exception($"{resp} is not a valid item.");

    }
    #endregion

    #region Inner classes
    private class Flags {
      public const string UseProduction = "/production";
    }
    #endregion
  }
}
