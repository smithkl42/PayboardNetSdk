using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Payboard.Sdk.Infrastructure
{
    public static class Requestor
    {
        public static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(PayboardConfiguration.BaseUrl);            
            return client;
        }
    }
}