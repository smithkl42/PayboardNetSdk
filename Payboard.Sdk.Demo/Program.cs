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
        public static void AddEvents(string commandText)
        {
            String connString = "Server=tcp:ubch569rcn.database.windows.net,1433;Database=socedo_db_beta;User ID=socedo_readonly_login@ubch569rcn;Password=GoPayboard!123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connString);

            //Open connection
            conn.Open();

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
                    event1.CustomerName = dataReader.GetString(1);
                    event1.CustomerUserId = dataReader.GetInt32(2).ToString();
                    event1.CustomerUserEmail = dataReader.GetString(3);
                    event1.CustomerUserFirstName = dataReader.GetString(4);
                    event1.CustomerUserLastName = dataReader.GetString(5);
                    //event1.CustomerUserEmail = dataReader.GetString(3);
                    event1.EventName = dataReader.GetString(6);
                    events.Add(event1);
                }

            }
            else
                Console.WriteLine("No rows returned.");

            //EventName = AddedCriteria
            //EventName = AuthorizedOauth
            //EventName = Premium 

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
            return; 
        }

        private static void Main(string[] args)
        {
            //make it check every N hours, and then run a cron job the same interval WHERE DateTime > GetDate()-1 day
            String commandText1 = "select  c.id as CustomerId, c.name as CustomerName, c.id as CustomerUserId, ac.UserName as Email, SUBSTRING(c.Name, 1, CHARINDEX(' ', c.Name)-1) AS FirstName, SUBSTRING( c.Name, CHARINDEX(' ',  c.Name) + 1, CHARINDEX('''',  c.Name)-CHARINDEX(' ', c.Name)-1) AS LastName, 'SocedoRegistration' as EventName from aspnet_Users u inner join AccessControl ac on u.UserName = ac.UserName inner join Community c on ac.CommunityID = c.id WHERE c.CreatedUtc > dateadd(day,-1, getdate())";
            AddEvents(commandText1);
            //added criteria
            String commandText2 = "select DISTINCT  c.id as CustomerId, c.name as CustomerName, c.id as CustomerUserId, ac.UserName as Email, '' as FirstName, '' AS LastName, 'AddKeywords' AS EventName from aspnet_Users u inner join AccessControl ac on u.UserName = ac.UserName inner join Community c on ac.CommunityID = c.id inner join AccountCriteria acr on c.id = acr.AccountID WHERE acr.AddedUtc > dateadd(day,-1, getdate())";
            AddEvents(commandText2);
            //Followed at least 20th lead
            String commandText3 = "select DISTINCT c.id as CustomerId, c.name as CustomerName, c.id as CustomerUserId, ac.UserName as Email, '' AS FirstName, '' AS LastName, 'ReleventLeadsMarked' AS EventName from aspnet_Users u inner join AccessControl ac on u.UserName = ac.UserName inner join Community c on ac.CommunityID = c.id inner join LeadEventSink ls on c.id = ls.AccountID WHERE ls.LastModifiedUtc > dateadd(day,-1, getdate()) and c.id in ( SELECT s.AccountID  FROM dbo.LeadEventSink s  WHERE EngagementStatus = N'open'  GROUP BY s.AccountID  HAVING COUNT(*) > 19)";
            AddEvents(commandText3);
            //Direct Messages
            String commandText4 = "select DISTINCT  c.id as CustomerId, c.name as CustomerName, c.id as CustomerUserId, ac.UserName as Email, '' as FirstName, '' AS LastName, 'DmTemplateSet' AS EventName from aspnet_Users u inner join AccessControl ac on u.UserName = ac.UserName inner join Community c on ac.CommunityID = c.id inner join MessageTemplate mt on c.id = mt.AccountID WHERE mt.CreatedOnUtc > dateadd(day,-1, getdate())";
            AddEvents(commandText4);
            Console.ReadLine();
        }
    }
}