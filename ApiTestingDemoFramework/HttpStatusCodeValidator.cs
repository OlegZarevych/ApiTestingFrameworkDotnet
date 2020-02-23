using RestSharp;
using System;
using System.Net;

namespace ApiTestingDemoFramework
{
    public class HttpStatusCodeValidator
    {
        public static void CheckResponseCodeIsOk(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Wrong response code. ER : {HttpStatusCode.OK}, AR : {response.StatusCode}. Message - {response.Content}");
            }
        }
    }

}