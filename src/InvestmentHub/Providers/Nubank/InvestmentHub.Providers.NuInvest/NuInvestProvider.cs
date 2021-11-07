using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using InvestmentHub.Models;
using InvestmentHub.Providers.Models.NuInvest;
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

            await Authenticate(userName, userPassword, code);

            return _authToken != null;
        }

        public async Task<IEnumerable<Asset>> GetAssetsAsync(CancellationToken cancellationToken)
        {
            var getPositionResponse = await _httpClient.GetWithAuthorizationAsync<GetPositionResponse>(ProviderUrls.GET_POSITION, _authToken, cancellationToken);
            var assets = new List<Asset>
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
            assets.AddRange(getPositionResponse.Investments?.Select(GetAssetsFromPosition));

            return assets
                .Where(a => a != null);
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
        private async Task Authenticate(string userName, string userPassword, string optCode = null)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
            });

            using var page = await browser.NewPageAsync();
            await page.SetRequestInterceptionAsync(true);
            page.Request += async (sender, e) =>
            {
                var authenticationRequest = new AuthenticationRequest(userName, userPassword, optCode);
                var payload = new Payload
                {
                    Method = HttpMethod.Post,
                    Headers = authenticationRequest.GetHeaders(),
                    PostData = authenticationRequest.BuildFormValues(),
                };
                await e.Request.ContinueAsync(payload);
            };
            var response = await page.GoToAsync(ProviderUrls.AUTHENTICATE);
            var responseAuthenticate = await response.JsonAsync<GetAuthenticationResponse>();

            var responseJson = await response.JsonAsync();
            _authToken = responseJson.Value<string>("access_token");

            await browser.CloseAsync();
        }

        private Asset GetAssetsFromPosition(Investment investment)
        {
            if (investment.GrossValue == 0)
            {
                return null;
            }

            var asset = new Asset
            {
                ProviderName = ProviderName,
                GeneratesIncome = true,
                Value = investment.GrossValue,
                Type = investment.InvestmentType.GetEquivalentAssetType(),
            };

            switch (investment.InvestmentType.Id)
            {
                case InvestmentTypeEnum.TREASURY:
                case InvestmentTypeEnum.FIXED_INCOME:
                    asset.Id = $"{ProviderName}:{investment.CustodyId}";
                    asset.AssetName = $"{investment.SecurityNameType} {investment.NickName} - {investment.Rentability}";
                    return asset;

                case InvestmentTypeEnum.STOCK:
                case InvestmentTypeEnum.ETF:
                case InvestmentTypeEnum.FII:
                    asset.Id = $"{ProviderName}:{investment.StockCode}";
                    asset.AssetName = $"{investment.SecurityType} {investment.NickName}";
                    return asset;

                default:
                    asset.Id = $"{ProviderName}:{investment.CustodyId}";
                    asset.AssetName = $"{investment.SecurityType} {investment.NickName}";
                    return asset;
            }
        }
    }
}
