# TipsTrade-HMRC
A strongly type .Net client for interacting with the HMRC APIs.
The following APIs are supported:
* Hello World
* VAT (MTD)
* Create Test User

## HMRC Developer Account
A HMRC developer account is required - [Login Here][1]. To make requests you need to create an application which provides the following credentials:
* Client ID
* Client secret
* Server token

You will also need to configure at least one Redirect URI and add API Subsciptions. To run tests and the `Authentication-Client` console app, the Solution requires a Redirect URI of `https://www.example.com/hmrc/callback`

## Creating test users
The Solution comes with a .Net Core Console App (`Authentication-Client`) which can be configured to run in either the Sandbox (default) or Production environment. Before it can be run it needs the user-secrets need to be populated:
```bash
dotnet user-secrets set Production:ServerToken <Sandbox Server Token>
dotnet user-secrets set Production:ClientSecret <Sandbox<Client Secret>
dotnet user-secrets set Production:ClientID <Sandbox Client ID>

dotnet user-secrets set Sandbox:ServerToken <Sandbox Server Token>
dotnet user-secrets set Sandbox:ClientSecret <Sandbox<Client Secret>
dotnet user-secrets set Sandbox:ClientID <Sandbox Client ID>

dotnet user-secrets -v list
```
By default, the `Authentication-Client` will run in the Sandbox environment. To run in Production add the `/production` argument.

## Test Configuration
For running test, you will need to provide the credentials. These are the same as the `Authentication-Client`. A `hmrc-users.json` is also required, this is empty by default and needs to be populated with the output of `Authentication-Client` for each user type.
```json
{
  "Agent": {
    "User": null,
    "Tokens": null
  },
  "Individual": {
    "User": null,
    "Tokens": null
  },
  "Organisation": {
    "User": null,
    "Tokens": null
  }
}
```

## OATH
To handle authentication, you need to create a URI to which the HMRC user is taken, so they can authenticate. Once the user has succesfully authenticated, they're redirected to a URI which is then handled by the client. If successfully handled, the client will return TokenResponse objects which contains the access and refresh tokens. The method below is similar to the GetAuthCode method in the Authentication-Client project, and outputs the TokenResponse.

```C#
private static TokenResponse GetAuthCode(Client client) {
  var state = $"{Guid.NewGuid()}";
  var scopes = Scopes.GetScopes();
  var redirectUrl = Configuration["RedirectUrl"];
  var url = client.GetAuthorizatoinEndpoint(state, redirectUrl, scopes);

  Console.WriteLine();
  Console.WriteLine("Navigate to the link below, and login:");
  Console.WriteLine($"\t{url}");

  Console.WriteLine();
  Console.WriteLine($"Paste in the '{redirectUrl}' address that you were redirected to:");
  var redirectedTo = Console.ReadLine();

  Console.WriteLine();
  Console.Write("Validating...");
  var resp = client.HandleEndpointResult(redirectedTo, state);
  Console.WriteLine(" done");

  Console.WriteLine();
  Console.ForegroundColor = ConsoleColor.Green;
  Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
  Console.ResetColor();
  Console.Write("");

  return resp;
}
```

Refreshing the access token is done by simply passing the stored refresh token to the Client.RefreshAccessToken method:

```C#
private static void RefreshToken(Client client) {
  Console.WriteLine();
  Console.Write("Enter the refresh token: ");
  var refreshToken = Console.ReadLine();
  var tokens = client.RefreshAccessToken(refreshToken);

  Console.WriteLine();
  Console.ForegroundColor = ConsoleColor.Green;
  Console.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
  Console.ResetColor();
  Console.Write("");
}
```

[1]: https://developer.service.hmrc.gov.uk/developer/login
