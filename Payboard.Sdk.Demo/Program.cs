using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Payboard.Sdk.Entities;
using Payboard.Sdk.Services;

namespace Payboard.Sdk.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string connString =
                "Server=tcp:x8al0jxqwf.database.windows.net,1433;Database=PayboardProdDb;User ID=PayGrid@x8al0jxqwf;Password=3Edy26Pr95757ki;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var conn = new SqlConnection(connString);
            var service = new EventService();
            service.GetLastSynchronizedOn().ContinueWith(syncResult =>
            {
                // Not used at the moment - just showing how it's done.
                var lastSynchronizedOn = syncResult.Result;
                Console.WriteLine("Last synchronized on: {0}", lastSynchronizedOn);
            });

            service.GetLastSyncToken().ContinueWith(syncResult =>
            {
                // Not used at the moment - just showing how it's done.
                var lastSynctoken = syncResult.Result;
                Console.WriteLine("Last synchronized on: {0}", lastSynctoken);
            });

            //Open connection
            conn.Open();
            const string commandText = "SELECT top 5 * from [user] where firstname='Matt'";
            var command = new SqlCommand(commandText, conn);

            //select items from a database, and var them out
            var dataReader = command.ExecuteReader();

            //Display the results (if any)
            var events = new List<CustomerUserEvent>();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    var @event = new CustomerUserEvent();
                    @event.CustomerId = dataReader.GetInt32(0).ToString();
                    @event.CustomerName = dataReader.GetString(2) + " " + dataReader.GetString(3);
                    @event.CustomerUserId = dataReader.GetInt32(0).ToString();
                    @event.CustomerUserEmail = dataReader.GetString(4);
                    @event.CustomerUserFirstName = dataReader.GetString(2);
                    @event.CustomerUserLastName = dataReader.GetString(3);
                    @event.EventName = "TestEventRecorded";
                    events.Add(@event);
                    //Console.WriteLine("\t{0}\t\t{1}", dataReader.GetString(0), dataReader.GetString(1));
                }
            }
            else
                Console.WriteLine("No rows returned.");

            dataReader.Close();
            conn.Close();

            service.TrackCustomerUserEvents(events, Guid.NewGuid().ToString(), true).ContinueWith(result =>
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