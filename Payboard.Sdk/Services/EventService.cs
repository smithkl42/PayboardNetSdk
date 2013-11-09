﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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

        public Task TrackCustomerUserEvent(CustomerUserEvent @event)
        {
            return TrackCustomerUserEvents(new List<CustomerUserEvent> { @event });
        }

        public async Task TrackCustomerUserEvents(List<CustomerUserEvent> events)
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