using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using InvestmentHub.Models;
using InvestmentHub.Providers.Models.Nubank;
using InvestmentHub.Providers.Models.Nubank.Requests;
using InvestmentHub.Providers.Models.Nubank.Responses;

namespace InvestmentHub.Providers.Nubank
{
    public class NubankProvider : IProvider
    {
        public const string ProviderName = "Nubank";
        
        /// <summary>
        /// User Authentication Token
        /// </summary>
        private string _authToken;
        private readonly BaseHttpClient _httpClient;
        private readonly Endpoints _endpoints;

        public NubankProvider()
        {
            _httpClient = new HttpClient();
            _endpoints = new Endpoints(_httpClient);
        }

        /// <summary>
        /// Login to Nubank using QRCode authentication
        /// </summary>
        /// <param name="userName">User identifier on nubank, usually the CPF</param>
        /// <param name="userPassword">User password</param>
        /// <param name="code">The code represented by a QrCode, that have to be read by Nubank app</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// Returns 'true' for successful login and 'false' failed login
        /// </returns>
        public async Task<bool> LoginAsync(string userName, string userPassword, string code, CancellationToken cancellationToken)
        {
            await GetAuthTokenAsync(userName, userPassword, cancellationToken);
            
            if (string.IsNullOrEmpty(code))
            {
                return _endpoints.Events != null;
            }

            var body = new AuthenticationRequest
            {
                QrCodeId = code,
                Type = "login-webapp"
            };

            var liftUrl = await _endpoints.GetLiftUrl(cancellationToken);
            var response = await _httpClient.PostWithAuthorizationAsync<AuthenticationRequest, AuthenticationResponse>(liftUrl, body, _authToken, cancellationToken);

            FillAuthToken(response);

            FillAuthenticatedUrls(response);

            return _endpoints.Events != null;
        }

        /// <summary>
        /// Get user credit card events on Nubank 
        /// </summary>
        /// <returns>List of events for user</returns>
        public async Task<IEnumerable<Event>> GetEventsAsync(CancellationToken cancellationToken)
        {
            EnsureAuthenticated();

            var response = await _httpClient.GetWithAuthorizationAsync<GetEventsResponse>(_endpoints.Events, _authToken, cancellationToken);
            
            if (!string.IsNullOrEmpty(response.Error))
            {
                throw new Exception(response.Error);
            }
            
            return response.Events;
        }

        public async Task<IEnumerable<Asset>> GetAssetsAsync(CancellationToken cancellationToken)
        {
            EnsureAuthenticated();

            var savingsRequest = new GetSavingsRequest();
            var getSavingsResponse = await _httpClient.PostWithAuthorizationAsync<GetSavingsRequest, GetSavingsResponse>(_endpoints.GraphQl, savingsRequest, _authToken, cancellationToken);
            if (!string.IsNullOrEmpty(getSavingsResponse.Error))
            {
                throw new Exception(getSavingsResponse.Error);
            }
            
            var getAccountsResponse = await _httpClient.GetWithAuthorizationAsync<GetAccountResponse>(_endpoints.Account, _authToken, cancellationToken);
            if (!string.IsNullOrEmpty(getAccountsResponse.Error))
            {
                throw new Exception(getAccountsResponse.Error);
            }

            var creditCardDebit = 0
                + getAccountsResponse.PrepaidCurrency
                - getAccountsResponse.OpenCurrency
                - getAccountsResponse.DueCurrency
                - getAccountsResponse.FutureCurrency;
            
            return new []
            {
                new Asset
                {
                    Id = $"{ProviderName}:Saldo",
                    ProviderName = ProviderName,
                    AssetName = "Saldo",
                    Type = AssetType.Balance,
                    GeneratesIncome = true,
                    Value = getSavingsResponse.NetAmount,
                },
                new Asset
                {
                    Id = $"{ProviderName}:Credit",
                    ProviderName = ProviderName,
                    AssetName = "Credit",
                    Type = AssetType.Credit,
                    GeneratesIncome = false,
                    Value = creditCardDebit,
                }
            };
        }

        /// <summary>
        /// Get authentication token using user Login and Password
        /// </summary>
        /// <returns></returns>
        private async Task GetAuthTokenAsync(string userName, string userPassword, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_authToken))
            {
                return;
            }

            var body = new AuthenticationRequest
            {
                ClientId = "other.conta",
                ClientSecret = "yQPeLzoHuJzlMMSAjC-LgNUJdUecx8XO",
                GrantType = "password",
                Login = userName,
                Password = userPassword
            };

            var loginUrl = await _endpoints.GetLoginUrlAsync(cancellationToken);

            var response = await _httpClient.PostAsync<AuthenticationRequest, AuthenticationResponse>(loginUrl, body, cancellationToken);

            FillAuthToken(response);

            FillAuthenticatedUrls(response);
        }

        /// <summary>
        ///  Extract authentication token from authentication request
        /// </summary>
        /// <param name="response">Response to the authentication request</param>
        private void FillAuthToken(AuthenticationResponse response)
        {
            if (!string.IsNullOrEmpty(response.AccessToken))
            {
                _authToken = response.AccessToken;
                return;
            }

            if (!string.IsNullOrEmpty(response.Error))
            {
                throw new AuthenticationException(response.Error);
            }
            throw new AuthenticationException("Unknown error on trying to authenticate to Nubank");
        }

        /// <summary>
        /// Get Authenticated Urls that will be used once the authentication is completed
        /// </summary>
        /// <param name="response"></param>
        private void FillAuthenticatedUrls(AuthenticationResponse response)
        {
            _endpoints.AuthenticatedUrls = response.Links;
        }

        /// <summary>
        /// Ensure the user is already authenticated
        /// </summary>
        private void EnsureAuthenticated()
        {
            if (string.IsNullOrEmpty(_authToken))
            {
                throw new Exception("Authentication needed");
            }
        }
    }
}
