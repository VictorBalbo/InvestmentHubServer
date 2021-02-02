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
        private AuthenticatedUrls _autenticatedUrls;
        public AuthenticatedUrls AutenticatedUrls { set => _autenticatedUrls = value; }

        public Endpoints(BaseHttpClient httpClient)
        {
            _client = httpClient;
        }

        /// <summary>
        /// Get Url to be used on login request
        /// </summary>
        /// <returns>Login Url</returns>
        public async Task<string> GetLoginUrlAsync() {
            if (string.IsNullOrEmpty(_webUrls?.Login))
            {
                _webUrls = await GetWebUrls();
            }
            return _webUrls.Login;
        }

        /// <summary>
        /// Get urls to be used on login with QRCode
        /// </summary>
        /// <returns>Lift Urls</returns>
        public async Task<string> GetLiftUrl()
        {
            if (string.IsNullOrEmpty(_appUrls?.Lift))
            {
                _appUrls = await GetAppUrls();
            }
            return _appUrls.Lift;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Events => _autenticatedUrls?.Events;
        public string GraphQl => _autenticatedUrls?.Ghostflame;


        private Task<WebUrls> GetWebUrls()
        {
            return _client.GetAsync<WebUrls>(DiscoveryUrl);
        }

        private Task<AppUrls> GetAppUrls()
        {
            return _client.GetAsync<AppUrls>(DiscoveryAppUrl);
        }
    }
}
