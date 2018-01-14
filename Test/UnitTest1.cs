using System;
using Xunit;
using ShoppingListApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ApplicationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public ApplicationTest()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(@"../../.."))
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(configuration));
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task UnAuthorizedAccess()
        {
            var response = await _client.GetAsync("/api/shoppinglist");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task GetToken()
        {
            var bodyString = @"{username: ""James"", password: ""Bond""}";
            var response = await _client.PostAsync("/api/token", new StringContent(bodyString, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            Assert.NotNull((string)responseJson["token"]);
        }

        [Fact]
        public async Task GetShoppingList()
        {
            var bodyString = @"{username: ""James"", password: ""Bond""}";
            var response = await _client.PostAsync("/api/token", new StringContent(bodyString, Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var token = (string)responseJson["token"];

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/shoppinglist");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ListResponse = await _client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.OK, ListResponse.StatusCode);
        }

        [Fact]
        public async Task AddShoppingList()
        {
            var bodyString = @"{username: ""James"", password: ""Bond""}";
            var response = await _client.PostAsync("/api/token", new StringContent(bodyString, Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var token = (string)responseJson["token"];

            var list = @"{ItemName: ""Test""}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/shoppinglist");
            requestMessage.Content = new StringContent(list, Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ListResponse = await _client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.Created, ListResponse.StatusCode);
        }

    }
}
