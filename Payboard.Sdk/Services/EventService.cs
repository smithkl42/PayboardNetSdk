using System.Collections.Generic;
using System.Net.Http;
using Payboard.Sdk.Entities;
using Payboard.Sdk.Infrastructure;

namespace Payboard.Sdk.Services
{
    public class EventService
    {
        public EventService()
        {
            _apiKey = PayboardConfiguration.GetPublicApiKey();
        }

        public EventService(string apiKey)
        {
            _apiKey = apiKey;
        }

        private readonly string _apiKey;

        public async void TrackCustomerUserEvents(List<CustomerUserEvent> events)
        {
            var client = Requestor.GetClient();
            var url = string.Format("/api/organizations/{0}/customeruserevents/", _apiKey);
            var response = await client.PostAsJsonAsync(url, events);
            if (!response.IsSuccessStatusCode)
            {
                throw new PayboardException(response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}