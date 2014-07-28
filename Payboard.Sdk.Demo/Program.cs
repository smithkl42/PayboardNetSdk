using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Payboard.Sdk.Entities;
using Payboard.Sdk.Services;

namespace Payboard.Sdk.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // RunDemoAsync().Wait();
            var task = GenerateLoadAsync(1000, 10);
            task.Wait();
            if (task.IsFaulted && task.Exception != null)
            {
                foreach (var ex in task.Exception.InnerExceptions)
                {
                    Console.WriteLine(ex);
                }
            }
            Console.ReadLine();
        }

        private static async Task RunDemoAsync()
        {
            const string connString =
    "Server=tcp:x8al0jxqwf.database.windows.net,1433;Database=PayboardProdDb;User ID=PayGrid@x8al0jxqwf;Password=3Edy26Pr95757ki;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var conn = new SqlConnection(connString);
            var service = new EventService();
            var lastSynchronizedOn = await service.GetLastSynchronizedOn();
            Console.WriteLine("Last synchronized on: {0}", lastSynchronizedOn);

            var lastSynctoken = await service.GetLastSyncToken();
            Console.WriteLine("Last synchronized on: {0}", lastSynctoken);

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
            {
                Console.WriteLine("No rows returned.");
            }

            dataReader.Close();
            conn.Close();

            await service.TrackCustomerUserEvents(events, Guid.NewGuid().ToString(), true);
            Console.WriteLine("The events were recorded");
        }

        private static async Task GenerateLoadAsync(int batches, int eventsPerBatch)
        {
            var sw = new Stopwatch();
            sw.Start();
            var service = new EventService();
            // var tasks = new List<Task>();
            for (var i = 0; i < batches; i++)
            {
                var events = new List<CustomerUserEvent>();
                var customerId = Guid.NewGuid().ToString();
                var customerUserid = Guid.NewGuid().ToString();
                for (var j = 0; j < eventsPerBatch; j++)
                {
                    var @event = new CustomerUserEvent();
                    @event.CustomerId = customerId;
                    @event.CustomerUserId = customerUserid;
                    @event.CustomerUserEmail = customerUserid + "@gmail.com";
                    @event.EventName = "TestEventRecorded";
                    events.Add(@event);
                }
                Console.WriteLine("Sending {0} events for customerUserId {1}", events.Count, customerUserid);
                await service.TrackCustomerUserEvents(events);
            }
            // Console.WriteLine("Waiting for {0} tasks to return", tasks.Count);
            // await Task.WhenAll(tasks);
            sw.Stop();
            Console.WriteLine("All tasks finished; took {0} seconds, or {1} seconds per event", 
                sw.Elapsed.TotalSeconds, sw.Elapsed.TotalSeconds / (batches * eventsPerBatch));
        }
    }
}