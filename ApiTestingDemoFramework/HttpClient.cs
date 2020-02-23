using RestSharp;
using System;

namespace ApiTestingDemoFramework
{
    public class HttpClient
    {
        private readonly RestClient client;

        public RestRequest Request { get; set; }

        public event Action<IRestResponse> CheckResponseCode;
        public HttpClient(string url)
        {
            client = new RestClient(url);
            Request = new RestRequest();
        }

        public void AddAuthorizationToken(string token)
        {
            client.AddDefaultHeader("Authorization", $"Bearer {token}");
        }

        public T SendPostAsJson<T>(object body, string relativeUrl)
        {
            Request.AddJsonBody(body);
            Request.Resource = relativeUrl;
            Request.Method = Method.POST;

            IRestResponse<T> response = client.ExecuteAsync<T>(Request).GetAwaiter().GetResult();

            CheckResponseCode?.Invoke(response);

            return response.Data;
        }

        public T SendGet<T>(string relativeUrl)
            where T : new()
        {
            Request.Resource = relativeUrl;
            IRestResponse<T> response = client.Get<T>(Request);

            CheckResponseCode?.Invoke(response);

            return response.Data;
        }
    }
}
