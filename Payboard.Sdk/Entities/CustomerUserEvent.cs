using System;

namespace Payboard.Sdk.Entities
{
    public class CustomerUserEvent
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerUserId { get; set; }
        public string CustomerUserFirstName { get; set; }
        public string CustomerUserLastName { get; set; }
        public string CustomerUserEmail { get; set; }
        public string EventName { get; set; }
        public DateTime? OccurredOn { get; set; }
        public string Url { get; set; }
        public string Params { get; set; }
    }
}