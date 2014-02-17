using System;
using System.Net;

namespace Payboard.Sdk.Infrastructure
{
    [Serializable]
    public class PayboardException : ApplicationException
    {
        public PayboardException()
        {
        }

        public PayboardException(HttpStatusCode httpStatusCode, string message)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}