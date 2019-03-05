# TipsTrade-HMRC
A strongly type .Net client for interacting with the HMRC APIs.
The following APIs are currently supported:
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
dotnet user-secrets set Production:ClientSecret <Sandbox Client Secret>
dotnet user-secrets set Production:ClientID <Sandbox Client ID>

dotnet user-secrets set Sandbox:ServerToken <Sandbox Server Token>
dotnet user-secrets set Sandbox:ClientSecret <Sandbox Client Secret>
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

## Scopes
Scopes are used to provide access to specific APIs, and have to be passed to the `Client.GetAuthorizatoinEndpoint` method. These are then presented to the end user so they can confirm what access your application is intending on receiving. Scopes also have to be enabled for specific [Applications][2] in the developer console.

Scopes can be specified by referencing the constants directly:
```C#
var scopes = new string[] {Scopes.Hello, Scopes.VATRead, Scopes.VATWrite};
```

In addition, the you can use the `Scopes.GetScopes` helper methods to retrieve scopes for a specific Api, or to filter scopes by name or value:
```C#
// Get all the possible scopes - you probably won't want to do this in production.
var scopes = Scopes.GetScopes();

// Get all the scopes for the specified Api
var scopes = Scopes.GetScopes<Api.Vat.VatApi>();

// All Scopes in the Scopes class that contains "VAT", eg. "VATRead", "VATWrite"
var scopes = Scopes.GetScopes(nameFilter: (name) => name.Contains("VAT"));

// All scopes in the Scopes class that are for the VAT api, and contain "read"
var scopes = Scopes.GetScopes<Api.Vat.VatApi>(valueFilter: (value) => value.Contains("read"));
```

## Creating the Api Client
The `Client` class requires the credentials before any Api methods can be called.
```C#
// Creates a client with production credentials
var client = new Client("Client ID", "Client secret", "Server token");

// Creates a client with sandbox credentials
var client = new Client("Client ID", "Client secret", "Server token", true);

// Or using the properties
var client = new Client() {
  ClientID = "Client ID",
  ClientSecret = "Client secret",
  ServerToken = "Server token",
  IsSandbox = true
};
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

## Invoking API methods
The APIs can be accessed their respective properties in the `Client` class
* CreateTestUser
* HelloWorld
* Vat

### Exception handling
All Api methods can throw a `ApiException`. The `ApiException.Message` property will contain the core error message returned by the API. To gain more specific information, use the `ApiException.ApiError` property, which in turn contains an optional list of `ErrorResponse` objects in the `ApiException.ApiError.Errors` property.
```C#
try {
  submitResponse = client.Vat.SubmitReturn(submitRequest);
  // ...
} catch (ApiException ex) {
  var message = ex.Message;
  var detailedMessage = string.Join("\r\n", ex.ApiError.Errors?.Select(x => x.Message));
}
``` 

### CreateTestUser API
Test users are required when using the Sandbox for testing and development. They will need to be created regularly as they are [cleared down every two weeks][3]. The simplest method is to use the `TipsTrade.HMRC.Api.CreateTestUser.CreateTestUserFactory<T>` static methods to create a `ICreateTestUserRequest` class:
```C#
// Create a test organisation user with all the available service types
var orgRequest = CreateTestUserFactory<CreateOrganisationRequest>.CreateTestUserFull();
 
// Create a test organisation user with only VAT services
var orgVatRequest = CreateTestUserFactory<CreateOrganisationRequest>.CreateTestUser(s => s.Contains("vat"));

var user = client.CreateTestUser.CreateUser(orgRequest);
```

### HelloWorld API
A simple echo service, to verify that application or user credentials are valid:
```C#
// Returns "Hello World" - no application or user credentials are required
var resp = client.HelloWorld.SayHelloWorld();

// Returns "Hello Application" - application credentials are required
var resp = client.HelloWorld.SayHelloApplication();

// Returns "Hello User" - a valid user's access token is required
var tokens = client.RefreshAccessToken("Saved Refresh Token");
var resp = client.HelloWorld.SayHelloUser();
```

### VAT (MTD) API

#### Get obligations
VAT returns are indexed by a "Period Key", so it's necessary to retrieve the obligations. The `ObligationsResult` implements `IComparable`, so they can be sorted (using the `Start` property). **Note: HMRC specify that the Period Key should not be shown to the end user.**
```C#
// Get all fulfilled obligations for the past year
var obRequest = new ObligationsRequest() {
  Vrn = vrn,
  Status = ObligationStatus.Fulfilled, // Use null to request all obligations
  DateFrom = DateTime.Today.AddYears(-1),
  DateTo = DateTime.Today
};

var obligations = client.Vat.GetObligations(obRequest);

// The most recent fulfilled obligation
var periodKey = resp.Value?.OrderByDescending(x => x).FirstOrDefault()?.PeriodKey;
```

#### Get VAT return
```C#
var returnRequest = new ReturnRequest() {
  Vrn = vrn,
  PeriodKey = periodKey // As retrieved by GetObligations
};

var vatReturn = client.Vat.GetReturn(returnRequest);
```

#### Submit VAT return
As per the documentation, `Decimal` values cannot have more than two decimal places. It's advisable to use `Math.Round` or similar.
```C#
var vatReturn = new VatReturn() {
  PeriodKey = periodKey, // As retrieved by GetObligations
  VatDueSales = 7724.92m,
  VatDueAcquisitions = 703.49m,
  TotalVatDue = 7724.92m + 703.49m,
  VatReclaimedCurrPeriod = 1681.08m,
  NetVatDue = 7724.92m + 703.49m - 1681.08m,
  TotalValueSalesExVAT = 38622,
  TotalValuePurchasesExVAT = 8405,
  TotalValueGoodsSuppliedExVAT = 3703,
  TotalAcquisitionsExVAT = 500,
  Finalised = true // Must be true for the return to be accepted
};

var request = new SubmitRequest() {
  Return = vatReturn,
  Vrn = vrn,
};

var resp = client.Vat.SubmitReturn(request);
```

#### Other methods
Awaiting documentation:
* `Client.Vat.GetLiabilities`
* `Client.Vat.GetPayments`

[1]: https://developer.service.hmrc.gov.uk/developer/login
[2]: https://developer.service.hmrc.gov.uk/developer/applications
[3]: https://developer.service.hmrc.gov.uk/api-documentation/docs/testing/data-cleardown