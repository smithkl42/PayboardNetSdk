using System;
using System.Configuration;

namespace Payboard.Sdk.Infrastructure
{
    public static class PayboardConfiguration
    {
        private static string _apiKey;

        internal static string GetPublicApiKey()
        {
            if (String.IsNullOrEmpty(_apiKey))
                _apiKey = ConfigurationManager.AppSettings["PayboardPublicApiKey"];

            return _apiKey;
        }

        public static void SetPublicApiKey(string newApiKey)
        {
            _apiKey = newApiKey;
        }
    }
}
