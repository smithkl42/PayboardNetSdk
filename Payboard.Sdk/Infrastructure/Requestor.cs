using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Payboard.Sdk.Infrastructure
{
    public static class Requestor
    {
        private const string BaseUrl = "https://app.payboard.com/";

        public static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}