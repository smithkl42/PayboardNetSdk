using System.Configuration;

namespace Payboard.Sdk.Infrastructure
{
    public static class PayboardConfiguration
    {
        private static string _apiKey;

        private static string _baseUrl;

        public static string PublicApiKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_apiKey))
                {
                    _apiKey = ConfigurationManager.AppSettings["PayboardPublicApiKey"];
                }
                return _apiKey;
            }
            set { _apiKey = value; }
        }

        public static string BaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_baseUrl))
                {
                    _baseUrl = ConfigurationManager.AppSettings["PayboardBaseUrl"];
                }
                return _baseUrl;
            }
            set { _baseUrl = value; }
        }

        public static void SetPublicApiKey(string newApiKey)
        {
            _apiKey = newApiKey;
        }
    }
}