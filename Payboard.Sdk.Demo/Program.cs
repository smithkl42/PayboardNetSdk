using System;
using System.Collections.Generic;
using Payboard.Sdk.Entities;
using Payboard.Sdk.Services;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace Payboard.Sdk.Demo
{
    internal class Program
    {
        private static string userName = "<ProvideUserName>";
        private static string password = "<ProvidePassword>";
        private static string dataSource = "<ProvideServerName>";
        private static string sampleDatabaseName = "<ProvideDatabaseName>";

        private static void Main(string[] args)
        {


            String connString = "Server=tcp:x8al0jxqwf.database.windows.net,1433;Database=PayboardProdDb;User ID=PayGrid@x8al0jxqwf;Password=3Edy26Pr95757ki;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connString);

            //Open connection
            conn.Open();
            String commandText = "SELECT top 5 * from [user] where firstname='Matt'";
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
                    event1.CustomerName = dataReader.GetString(2) + " " + dataReader.GetString(3);
                    event1.CustomerUserId = dataReader.GetInt32(0).ToString();
                    event1.CustomerUserEmail = dataReader.GetString(4);
                    event1.CustomerUserFirstName = dataReader.GetString(2);
                    event1.CustomerUserLastName = dataReader.GetString(3);
                    event1.EventName = "TestEventRecorded";
                    events.Add(event1);
                    //Console.WriteLine("\t{0}\t\t{1}", dataReader.GetString(0), dataReader.GetString(1));
                }
                    
            }
            else
                Console.WriteLine("No rows returned.");

            //Close the data reader
            dataReader.Close();

            //Close the database connection
            conn.Close();


            


            var service = new EventService();
            
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