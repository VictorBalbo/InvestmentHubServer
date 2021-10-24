using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InvestmentHub.Models;
using InvestmentHub.Providers.Models.NuInvest.Requests;
using PuppeteerSharp;

namespace InvestmentHub.Providers.NuInvest
{
    public class NuInvestProvider : IProvider
    {
        public const string ProviderName = "NuInvest";
        private HttpClient _httpClient;
        private RevisionInfo _browserRevisionInfo;

        /// <summary>
        /// User Authentication Token
        /// </summary>
        private string _authToken;

        public NuInvestProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> LoginAsync(string userName, string userPassword, string code, CancellationToken cancellationToken)
        {
            if (_browserRevisionInfo == null)
            {
                await DownloadBrowser();
            }

            await Authenticate(userName, userPassword);

            return _authToken != null;
        }

        public async Task<IEnumerable<Asset>> GetAssetsAsync(CancellationToken cancellationToken)
        {
            var getPositionResponse = await _httpClient.GetWithAuthorizationAsync<GetPositionResponse>(ProviderUrls.GET_POSITION, _authToken, cancellationToken);

            return new[]
            {
                new Asset
                {
                    Id = $"{ProviderName}:Saldo",
                    ProviderName = ProviderName,
                    AssetName = "Saldo",
                    Type = AssetType.Balance,
                    GeneratesIncome = false,
                    Value = getPositionResponse.EasyBalance,
                },
            };
        }

        private async Task DownloadBrowser()
        {
            _browserRevisionInfo = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        }

        /// <summary>
        /// NuInvest has a validation against bots, so the login for this provider is done by browser simulation
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        private async Task Authenticate(string userName, string userPassword)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
            });

            var page = await browser.NewPageAsync();
            await page.GoToAsync(ProviderUrls.AUTHENTICATE);
            await page.TypeAsync(AuthenticationRequest.UserNameSelector, userName);
            await page.TypeAsync(AuthenticationRequest.PasswordSelector, userPassword);
            await page.Keyboard.PressAsync(AuthenticationRequest.EnterKey);
            await ExtractAuthToken(page);

            if (_authToken == null)
            {
                await page.WaitForNavigationAsync();
                await ExtractAuthToken(page);
            }

            await browser.CloseAsync();
        }

        private async Task ExtractAuthToken(Page page)
        {
            var localStorage = await page.EvaluateFunctionAsync<Dictionary<string, string>>("async () => Object.assign({}, window.localStorage)");
            localStorage?.TryGetValue("access_token", out _authToken);
        }
    }
}
