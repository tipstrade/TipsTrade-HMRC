# TipsTrade-HMRC
A strongly type .Net client for interacting with the HMRC APIs

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

[1]: https://developer.service.hmrc.gov.uk/developer/login
