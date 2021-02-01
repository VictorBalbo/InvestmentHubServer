using InvestmentHub.Models;
using InvestmentHub.Providers.Models.Rico;
using InvestmentHub.Providers.Models.Rico.Requests;
using InvestmentHub.Providers.Models.Rico.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.Providers.Rico
{
    public sealed class RicoProvider : IProvider, IDisposable
    {
        public string ProviderName { get; }
        private readonly BaseHttpClient _httpClient;

        public RicoProvider()
        {
            ProviderName = "Rico";
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Asset>> GetAssetsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var getSummaryResponse = await _httpClient.GetAsync<GetSummaryPositionResponse>(ProviderUrls.GET_SUMMARY_POSITION, cancellationToken);
                var assets = await Task.WhenAll(getSummaryResponse.Positions.Select(p => GetAssetsFromPositionAsync(p, cancellationToken)));
                return assets
                    .Where(a => a?.Count() > 0)
                    .SelectMany(a => a);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting savings from Rico Provider");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> LoginAsync(string userName, string userPassword, CancellationToken cancellationToken)
        {
            try
            {
                var getKeyboardRequest = new GetKeyboardRequest
                {
                    Username = userName,
                    SessionId = Guid.NewGuid().ToString(),
                };
                var getKeyboardResponse =
                    await _httpClient.PostAsync<GetKeyboardRequest, GetKeyboardResponse>(ProviderUrls.GET_KEYBOARD, getKeyboardRequest, cancellationToken);

                // Use the keyboard to complete the authentication
                var authenticateRequest = new AuthenticateRequest
                {
                    Username = userName,
                    SessionId = getKeyboardRequest.SessionId,
                    Token = getKeyboardResponse.Token,
                    Password = GetPasswordFromKeyboard(userPassword, getKeyboardResponse)
                };
                await _httpClient.PostAsync(ProviderUrls.AUTHENTICATE, authenticateRequest, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error authenticating to Rico Provider");
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private string GetPasswordFromKeyboard(string userPassword, GetKeyboardResponse keyboard)
        {
            var password = userPassword.Select(
                c => keyboard.Keys
                    .First(kp => kp.Value.Contains($"{c}"))
                    .Key
                );
            return string.Join("", password);
        }

        private async Task<IEnumerable<Asset>> GetAssetsFromPositionAsync(Position position, CancellationToken cancellationToken)
        {
            if (position.NetValue == 0)
            {
                return null;
            }

            switch (position.ProductType)
            {
                case ProductType.BALANCE:
                    return new Asset[]
                    {
                        new Asset
                        {
                            Id = $"{ProviderName}:{position.ProductTypeName}",
                            ProviderName = ProviderName,
                            AssetName = position.ProductTypeName,
                            GeneratesIncome = false,
                            Value = position.NetValue,
                            Type = position.ProductType.GetEquivalentAssetType(),
                        }
                    };

                case ProductType.TREASURY:
                    return new Asset[]
                    {
                        new Asset
                        {
                            Id = $"{ProviderName}:{position.ProductTypeName}",
                            ProviderName = ProviderName,
                            AssetName = position.ProductTypeName,
                            GeneratesIncome = true,
                            Value = position.NetValue,
                            Type = position.ProductType.GetEquivalentAssetType(),
                        }
                    };

                default:
                    try
                    {
                        var requestUrl = ProviderUrls.GET_POSITION_DETAILS.Replace("{TYPE}", position.ProductTypeString.ToLower());
                        var positionDetailResponse = await _httpClient.GetAsync<GetPositionDetailResponse>(requestUrl, cancellationToken);
                        return positionDetailResponse.Positions.Select(pd =>
                            new Asset
                            {
                                Id = $"{ProviderName}:{pd.Symbol.Name}",
                                ProviderName = ProviderName,
                                AssetName = pd.Symbol.Name,
                                GeneratesIncome = true,
                                Value = pd.TotalValue,
                                Type = position.ProductType.GetEquivalentAssetType(),
                            }
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error authenticating to Rico Provider");
                        Console.WriteLine(ex.Message);
                        return new Asset[]
                        {
                            new Asset
                            {
                                Id = $"{ProviderName}:{position.ProductTypeName}",
                                ProviderName = ProviderName,
                                AssetName = position.ProductTypeName,
                                GeneratesIncome = true,
                                Value = position.NetValue,
                                Type = position.ProductType.GetEquivalentAssetType(),
                            }
                        };
                    }
            }
        }
    }
}
