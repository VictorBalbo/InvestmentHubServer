using System.Collections.Generic;

namespace InvestmentHub.Providers.NuInvest
{
    public class HttpClient : BaseHttpClient
    {
        protected override IDictionary<string, string> GetDefaultHeaders()
        {
            return new Dictionary<string, string>
            {
                { "accept", "*/*" },
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.102 Safari/537.36" },
            };
        }

        protected override string GetAuthorizationHeader(string authToken)
            => $"Bearer {authToken}";
    }
}
