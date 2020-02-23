using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiTestingDemo.Models;
using NUnit;
using NUnit.Framework;
using RestSharp;
using Shouldly;

namespace ApiTestingDemo
{
    [TestFixture]
    public class HttpTestByRestSharp
    {
        [Test]
        public void AutheticationRequest_MethodGet_ShouldBeNotAllowed()
        {
            RestClient client = new RestClient("https://exploresolutionapi.azurewebsites.net/");

            RestRequest request = new RestRequest("/api/Authentication/request");

            request.Method = Method.GET;

            IRestResponse response = client.Execute(request);

            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);

        }

        [Test]
        public async Task GetAllTours_ValidData_ShouldReturnTours()
        {
            RestClient client = new RestClient();

            RestRequest authenticationRequest = new RestRequest("https://exploresolutionapi.azurewebsites.net/api/Authentication/request");

            AuthModel authentication = new AuthModel();
            authentication.Username = "user";
            authentication.Password = "pass";

            authenticationRequest.AddJsonBody(authentication);

            IRestResponse tokenResponse = client.Post(authenticationRequest);

            tokenResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            tokenResponse.Content.ShouldNotBeNull();


            RestRequest getAllTours = new RestRequest("https://exploresolutionapi.azurewebsites.net/api/tours/getAll");
            getAllTours.AddHeader("Authorization", $"Bearer {tokenResponse.Content.Trim('"')}");

            List<Tour> getAllToursResponse = await client.GetAsync<List<Tour>>(getAllTours);

            getAllToursResponse.Count.ShouldBe(2);
        }
    }
}
