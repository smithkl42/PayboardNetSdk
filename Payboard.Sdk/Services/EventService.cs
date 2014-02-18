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
        private readonly string _apiKey;

        public EventService()
        {
            _apiKey = PayboardConfiguration.PublicApiKey;
            Timeout = TimeSpan.FromMinutes(10);
        }

        public EventService(string apiKey):this()
        {
            _apiKey = apiKey;
        }

        public TimeSpan Timeout { get; set; }

        public Task TrackCustomerUserEvent(CustomerUserEvent @event, string syncToken= null, bool setSynchronizedOn = false)
        {
            return TrackCustomerUserEvents(new List<CustomerUserEvent> {@event}, syncToken, setSynchronizedOn);
        }

        public async Task TrackCustomerUserEvents(List<CustomerUserEvent> events, string syncToken = null, bool setSynchronizedOn = false)
        {
            var client = Requestor.GetClient(Timeout.TotalMilliseconds);
            var url = string.Format("/api/organizations/{0}/customeruserevents/?setSynchronizedOn={1}&syncToken={2}", 
                _apiKey, setSynchronizedOn, syncToken);
            var response = await client.PostAsJsonAsync(url, events);
            if (!response.IsSuccessStatusCode)
            {
                throw new PayboardException(response.StatusCode, response.ReasonPhrase);
            }
        }

        public async Task UploadCsv(string csv, bool hasHeaders = false, string syncToken = null, bool setSynchronizedOn = false)
        {
            var client = Requestor.GetClient(Timeout.TotalMilliseconds);
            var url = string.Format("/api/organizations/{0}/customerusereventscsv/?setSynchronizedOn={1}&syncToken={2}&hasHeaders={3}",
                _apiKey, setSynchronizedOn, syncToken, hasHeaders);
            var response = await client.PostAsJsonAsync(url, csv);
            if (!response.IsSuccessStatusCode)
            {
                throw new PayboardException(response.StatusCode, response.ReasonPhrase);
            }
        }

        public async Task<DateTime?> GetLastSynchronizedOn()
        {
            var client = Requestor.GetClient(Timeout.TotalMilliseconds);
            var url = string.Format("/api/organizations/{0}/lastsynchronizedon", _apiKey);
            var response = await client.GetStringAsync(url);
            var dateString = JsonConvert.DeserializeObject<string>(response);
            DateTime lastSynchronizedOn;
            if (DateTime.TryParse(dateString, out lastSynchronizedOn))
            {
                return lastSynchronizedOn;
            }
            return null;
        }

        public async Task<string> GetLastSyncToken()
        {
            var client = Requestor.GetClient(Timeout.TotalMilliseconds);
            var url = string.Format("/api/organizations/{0}/lastsynctoken", _apiKey);
            var response = await client.GetStringAsync(url);
            var syncToken = JsonConvert.DeserializeObject<string>(response);
            return syncToken;
        }
    }
}