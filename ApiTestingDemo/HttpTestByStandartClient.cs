using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using NUnit.Framework;
using Shouldly;
using System.Net;
using ApiTestingDemo.Models;
using Newtonsoft.Json;

namespace ApiTestingDemo
{
    [TestFixture]
    public class HttpTestByStandartClient
    {
        [Test]
        public async Task AutheticationRequest_MethodGet_ShouldBeNotAllowed()
        {
            var client = new HttpClient();

            var result = await client.GetAsync("https://exploresolutionapi.azurewebsites.net//api/Authentication/request");

            result.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        [Test]
        public async Task GetAllTours_ValidData_ShouldReturnTours()
        {
            HttpRequestMessage authenticationRequest = new HttpRequestMessage(HttpMethod.Post, "https://exploresolutionapi.azurewebsites.net//api/Authentication/request");

            AuthModel authentication = new AuthModel();
            authentication.Username = "user";
            authentication.Password = "pass";

            string json = JsonConvert.SerializeObject(authentication, Formatting.Indented);

            authenticationRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage authenticationResponse;

            using (var client = new HttpClient())
            {
                authenticationResponse = await client.SendAsync(authenticationRequest);
            }

            authenticationResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            authenticationResponse.Content.ShouldNotBeNull();

            string token = await authenticationResponse.Content.ReadAsStringAsync();

            HttpRequestMessage getAllTours = new HttpRequestMessage(HttpMethod.Get, "https://exploresolutionapi.azurewebsites.net/api/tours/getAll");
            getAllTours.Headers.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage getAllToursResponse;

            using (var client = new HttpClient())
            {
                getAllToursResponse = await client.SendAsync(getAllTours);
            }

            getAllToursResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getAllToursResponse.Content.ShouldNotBeNull();

            string responseAsString = await getAllToursResponse.Content.ReadAsStringAsync();

            List<Tour> tourList = JsonConvert.DeserializeObject<List<Tour>>(responseAsString);
        }

    }
}
