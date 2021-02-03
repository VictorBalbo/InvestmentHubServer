using System.Threading;
using System.Threading.Tasks;
using InvestmentHub.Providers.Models.Nubank.Urls;

namespace InvestmentHub.Providers.Nubank
{
    public class Endpoints
    {
        private const string DiscoveryUrl = "https://prod-s0-webapp-proxy.nubank.com.br/api/discovery";
        private const string DiscoveryAppUrl = "https://prod-s0-webapp-proxy.nubank.com.br/api/app/discovery";
        private readonly BaseHttpClient _client;
        private WebUrls _webUrls;
        private AppUrls _appUrls;
        private AuthenticatedUrls _authenticatedUrls;
        public AuthenticatedUrls AuthenticatedUrls { set => _authenticatedUrls = value; }

        public Endpoints(BaseHttpClient httpClient)
        {
            _client = httpClient;
        }

        /// <summary>
        /// Get Url to be used on login request
        /// </summary>
        /// <returns>Login Url</returns>
        public async Task<string> GetLoginUrlAsync(CancellationToken cancellationToken) {
            if (string.IsNullOrEmpty(_webUrls?.Login))
            {
                _webUrls = await GetWebUrls(cancellationToken);
            }
            return _webUrls.Login;
        }

        /// <summary>
        /// Get urls to be used on login with QRCode
        /// </summary>
        /// <returns>Lift Urls</returns>
        public async Task<string> GetLiftUrl(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_appUrls?.Lift))
            {
                _appUrls = await GetAppUrls(cancellationToken);
            }
            return _appUrls.Lift;
        }
        
        public string Events => _authenticatedUrls?.Events;
        public string Account => _authenticatedUrls?.Account;
        public string GraphQl => _authenticatedUrls?.Ghostflame;


        private Task<WebUrls> GetWebUrls(CancellationToken cancellationToken)
        {
            return _client.GetAsync<WebUrls>(DiscoveryUrl, cancellationToken);
        }

        private Task<AppUrls> GetAppUrls(CancellationToken cancellationToken)
        {
            return _client.GetAsync<AppUrls>(DiscoveryAppUrl, cancellationToken);
        }
    }
}
