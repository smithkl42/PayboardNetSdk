using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Payboard.Sdk.Entities;
using Payboard.Sdk.Infrastructure;

namespace Payboard.Sdk.Services
{
    public class EventService
    {
        public EventService()
        {
            _apiKey = PayboardConfiguration.PublicApiKey;
        }

        public EventService(string apiKey)
        {
            _apiKey = apiKey;
        }

        private readonly string _apiKey;

        public Task TrackCustomerUserEvent(CustomerUserEvent @event, bool setSynchronizedOn = false)
        {
            return TrackCustomerUserEvents(new List<CustomerUserEvent> { @event }, setSynchronizedOn);
        }

        public async Task TrackCustomerUserEvents(List<CustomerUserEvent> events, bool setSynchronizedOn = false)
        {
            var client = Requestor.GetClient();
            var url = string.Format("/api/organizations/{0}/customeruserevents/?setSynchronizedOn={1}", _apiKey, setSynchronizedOn);
            var response = await client.PostAsJsonAsync(url, events);
            if (!response.IsSuccessStatusCode)
            {
                throw new PayboardException(response.StatusCode, response.ReasonPhrase);
            }
        }

        public async Task<DateTime?> GetLastSynchronizedOn()
        {
            var client = Requestor.GetClient();
            var url = string.Format("/api/organizations/{0}/lastsynchronizedon", _apiKey);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new PayboardException(response.StatusCode, response.ReasonPhrase);
            }
            var content = await response.Content.ReadAsStringAsync();
            var dateString = JsonConvert.DeserializeObject<string>(content);
            if (!string.IsNullOrWhiteSpace(dateString))
            {
                DateTime lastSynchronizedOn;
                if (DateTime.TryParse(dateString, out lastSynchronizedOn))
                {
                    return lastSynchronizedOn;
                }
            }
            return null;
        }
    }
}