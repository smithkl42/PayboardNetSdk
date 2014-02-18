using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Payboard.Sdk.Infrastructure
{
    public static class Requestor
    {
        public static HttpClient GetClient(double timeoutInMs = 10 * 60 * 1000)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeoutInMs);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(PayboardConfiguration.BaseUrl);
            return client;
        }
    }
}