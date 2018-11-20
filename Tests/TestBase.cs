﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using TipsTrade.HMRC.Api.CreateTestUser.Model;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class TestBase {
    protected IConfiguration Configuration { get; }

    protected ITestOutputHelper Output { get; }

    #region State properties
    protected string AccessToken => Configuration["AccessToken"];

    protected string RefreshToken => Configuration["RefreshToken"];

    protected string State => Configuration["State"];
    #endregion

    #region User properties
    protected AgentResult AgentUser { get; private set; }

    protected IndividualResult IndividualUser { get; private set; }

    protected OrganisationResult OrganisationUser { get; private set; }
    #endregion

    #region Client properties
    protected string ClientId => Configuration["ClientID"];

    protected string ClientSecret => Configuration["ClientSecret"];

    protected bool IsSandbox => true;

    protected string ServerToken => Configuration["ServerToken"];
    #endregion

    public TestBase(ITestOutputHelper output) {
      Output = output;
      var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.secrets.json")
        .AddJsonFile("appsettings.tokens.json")
        ;

      Configuration = builder.Build();

      LoadUsersFromJsonFile();
    }

    protected Client GetClient() => new Client(ClientId, ClientSecret, ServerToken, IsSandbox);

    private void LoadUsersFromJsonFile() {
      var users = LoadFromJsonFile<JObject>("hmrc-users.json");
      AgentUser = users["agent"].ToObject<AgentResult>();
      IndividualUser = users["individual"].ToObject<IndividualResult>();
      OrganisationUser = users["organisation"].ToObject<OrganisationResult>();
    }

    private T LoadFromJsonFile<T>(string fileName) {
      using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
        using (var reader = new StreamReader(fs)) {
          return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }
      }
    }
  }
}
