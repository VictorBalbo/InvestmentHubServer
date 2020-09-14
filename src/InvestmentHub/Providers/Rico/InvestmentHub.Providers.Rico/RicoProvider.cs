using InvestmentHub.Providers.Models;
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
    public class RicoProvider : IProvider, IDisposable
    {
        public string ProviderName { get; }
        private readonly BaseHttpClient _httpClient;

        public RicoProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<InvestmentSummary> GetSavingsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var getSummaryResponse = await _httpClient.GetAsync<GetSummaryPositionResponse>(ProviderUrls.GET_SUMMARY_POSITION, cancellationToken);
                var investments = await Task.WhenAll(getSummaryResponse.Positions.Select(p => GetInvestmentFromPositionAsync(p, cancellationToken)));

                return new InvestmentSummary
                {
                    TotalValue = getSummaryResponse.TotalValue,
                    TotalInvestedValue = getSummaryResponse.TotalInvestedValue,
                    Investments = investments.Where(p => p?.Count() > 0).SelectMany(p => p)
                };
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
                    Token = getKeyboardResponse.Keyboard.Token,
                    Password = GetPasswordFromKeyboard(userPassword, getKeyboardResponse.Keyboard)
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

        private string GetPasswordFromKeyboard(string userPassword, Keyboard keyboard)
        {
            var password = userPassword.Select(
                c => keyboard.Keys
                    .First(kp => kp.Value.Contains($"{c}"))
                    .Key
                );
            return string.Join("", password);
        }

        private async Task<IEnumerable<Investment>> GetInvestmentFromPositionAsync(Position position, CancellationToken cancellationToken)
        {
            if (position.TotalValue == 0)
            {
                return null;
            }

            switch (position.ProductType)
            {
                case InvestmentType.BALANCE:
                    return new Investment[]
                    {
                        new Investment
                        {
                            ProviderName = ProviderName,
                            InvestmentName = position.ProductTypeName,
                            GeneratesIncome = false,
                            Value = position.TotalValue,
                            Type = position.ProductType,
                        }
                    };

                default:
                    try
                    {
                        var requestUrl = ProviderUrls.GET_POSITION_DETAILS.Replace("{TYPE}", position.ProductTypeString.ToLower());
                        var positionDetailResponse = await _httpClient.GetAsync<GetPositionDetailResponse>(requestUrl, cancellationToken);
                        return positionDetailResponse.Positions.Select(pd =>
                            new Investment
                            {
                                ProviderName = ProviderName,
                                InvestmentName = pd.Symbol.Name,
                                GeneratesIncome = true,
                                Value = pd.TotalValue,
                                Type = position.ProductType,
                            }
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error authenticating to Rico Provider");
                        Console.WriteLine(ex.Message);
                        return new Investment[]
                        {
                            new Investment
                            {
                                ProviderName = ProviderName,
                                InvestmentName = position.ProductTypeName,
                                GeneratesIncome = true,
                                Value = position.TotalValue,
                                Type = position.ProductType,
                            }
                        };
                    }
            }
        }
    }
}