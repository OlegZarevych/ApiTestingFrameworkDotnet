using ApiTestingDemoFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestingDemoFramework
{
    public class HttpService
    {
        private HttpClient httpClient;
        private readonly string absoluteUrl = "https://exploresolutionapi.azurewebsites.net";
        public string Token { get; set; } = string.Empty;

        public HttpService()
        {
        }

        public void Login(string username, string password)
        {
            httpClient = new HttpClient(absoluteUrl);

            AuthModel request = new AuthModel
            { Username = username, Password = password };

            var response = SendPostExpectedOk<AuthModel, string>(request, "/api/Authentication/request");

            Token += response;
        }

        public List<Tour> GetAllTours()
        {
            PrepareHttpClientForApiWithToken();

            return SendGetExpectedOk<List<Tour>>("/api/tours/getAll");
        }


        #region Helpers

        private V SendPostExpectedOk<T, V>(T request, string relativeUrl)
        {
            httpClient.CheckResponseCode += HttpStatusCodeValidator.CheckResponseCodeIsOk;

            V response = httpClient.SendPostAsJson<V>(request, relativeUrl);

            httpClient.CheckResponseCode -= HttpStatusCodeValidator.CheckResponseCodeIsOk;

            return response;
        }

        private T SendGetExpectedOk<T>(string relativeUrl)
            where T : new()
        {
            httpClient.CheckResponseCode += HttpStatusCodeValidator.CheckResponseCodeIsOk;

            T response = httpClient.SendGet<T>(relativeUrl);

            httpClient.CheckResponseCode -= HttpStatusCodeValidator.CheckResponseCodeIsOk;

            return response;
        }

        #endregion
        private void AddAuthorizationToken()
        {
            httpClient.AddAuthorizationToken(Token);
        }

        private void PrepareHttpClientForApiWithToken()
        {
            httpClient = new HttpClient(absoluteUrl);

            AddAuthorizationToken();
        }
    }
}