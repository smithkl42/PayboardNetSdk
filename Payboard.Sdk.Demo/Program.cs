using System;
using System.Collections.Generic;
using Payboard.Sdk.Entities;
using Payboard.Sdk.Services;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace Payboard.Sdk.Demo
{
    internal class Program
    {

        private static void Main(string[] args)
        {


            //String connString = "Server=tcp:x8al0jxqwf.database.windows.net,1433;Database=PayboardProdDb;User ID=PayGrid@x8al0jxqwf;Password=3Edy26Pr95757ki;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            String connString = "Server=tcp:ubch569rcn.database.windows.net,1433;Database=socedo_db_beta;User ID=socedo_readonly_login@ubch569rcn;Password=GoPayboard!123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connString);

            //Open connection
            conn.Open();

            //make it check every N hours, and then run a cron job the same interval WHERE DateTime > GetDate()-1 day
            String commandText = "SELECT top 5 AccountId, AccountId as FirstName, AccountId as LastName, AccountId as Email  from [AccountStatus] WHERE CreatedOnUtc > dateadd(day,-1, getdate())";


            SqlCommand command = new SqlCommand(commandText, conn);

            //select items from a database, and var them out
            SqlDataReader dataReader = command.ExecuteReader();

            //Display the results (if any)
            var events = new List<CustomerUserEvent>();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    var event1 = new CustomerUserEvent();
                    event1.CustomerId = dataReader.GetInt32(0).ToString();
                    event1.CustomerUserId = dataReader.GetInt32(0).ToString();
                    event1.CustomerName = "Bob Jones";
                    event1.CustomerUserFirstName = "Bob";
                    event1.CustomerUserLastName = "Jones";
                    event1.CustomerUserEmail = "bob@payboard.com";
                    //event1.CustomerUserEmail = dataReader.GetString(3);
                    event1.EventName = "TestEventRecorded";
                    events.Add(event1);
                }
                    
            }
            else
                Console.WriteLine("No rows returned.");

            //Close the data reader
            dataReader.Close();

            //Close the database connection
            conn.Close();


            


            var service = new EventService();
            //single event syntax
            //service.TrackCustomerUserEvent(events.First()).ContinueWith(result =>
            //    {
            //        if (result.IsFaulted)
            //        {
            //            Console.WriteLine(result.Exception);
            //        }
            //        else
            //        {
            //            Console.WriteLine("The event was recorded");
            //        }
            //    });

            //multiple event syntax
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