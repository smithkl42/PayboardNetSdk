using System;
using System.Collections.Generic;
using Payboard.Sdk.Entities;
using Payboard.Sdk.Services;

namespace Payboard.Sdk.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var event1 = new CustomerUserEvent();
            event1.CustomerId = "TestCustomerId";
            event1.CustomerName = "Test Customer";
            event1.CustomerUserId = "TestCustomerUserId1";
            event1.CustomerUserEmail = "bob@gmail.com";
            event1.CustomerUserFirstName = "Bob";
            event1.CustomerUserLastName = "Jones";
            event1.EventName = "TestEventRecorded";

            var event2 = new CustomerUserEvent();
            event2.CustomerId = "TestCustomerId";
            event2.CustomerName = "Test Customer";
            event2.CustomerUserId = "TestCustomerUserId2";
            event2.CustomerUserEmail = "jane@gmail.com";
            event2.CustomerUserFirstName = "Jane";
            event2.CustomerUserLastName = "Jones";
            event2.EventName = "TestEventRecorded";

            var service = new EventService();
            var events = new List<CustomerUserEvent> { event1, event2 };
            service.TrackCustomerUserEvents(events).ContinueWith(result =>
                {
                    if (result.IsFaulted)
                    {
                        Console.WriteLine(result.Exception);
                    }
                    else
                    {
                        Console.WriteLine("The events were recorded");
                    }
                });
            Console.ReadLine();
        }
    }
}