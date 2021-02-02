using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.Providers
{
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        protected BaseHttpClient()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // ReSharper disable once VirtualMemberCallInConstructor
            foreach (var (key, value) in GetDefaultHeaders())
            {
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
            }
        }

        public Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default)
            => SendRequest<object, TResponse>(url, HttpMethod.Get, null, null, cancellationToken);

        public Task<TResponse> GetWithAuthorizationAsync<TResponse>(string url, string authorization, CancellationToken cancellationToken = default)
            => SendRequest<object, TResponse>(url, HttpMethod.Get, null, authorization, cancellationToken);
        
        public Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest body, CancellationToken cancellationToken = default)
            => SendRequest<TRequest, TResponse>(url, HttpMethod.Post, body, null, cancellationToken);

        public Task PostAsync<TRequest>(string url, TRequest body, CancellationToken cancellationToken = default)
            => SendRequest<TRequest, object>(url, HttpMethod.Post, body, null, cancellationToken);
        
        public Task<TResponse> PostWithAuthorizationAsync<TRequest, TResponse>(string url, TRequest body, string authorization, CancellationToken cancellationToken = default)
            => SendRequest<TRequest, TResponse>(url, HttpMethod.Post, body, authorization, cancellationToken);
        
        public Task PostWithAuthorizationAsync<TRequest>(string url, TRequest body, string authorization, CancellationToken cancellationToken = default)
            => SendRequest<TRequest, object>(url, HttpMethod.Post, body, authorization, cancellationToken);

        private async Task<TResponse> SendRequest<TRequest, TResponse>(string url, HttpMethod method, TRequest body, string authorization = null, CancellationToken cancellationToken = default)
        {
            using var requestMessage = new HttpRequestMessage(method, url);
            
            if (!string.IsNullOrWhiteSpace(authorization))
            {
                var value = GetAuthorizationHeader(authorization);
                requestMessage.Headers.Add("Authorization", value);
            }
            
            if (body != null)
            {
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(body, _serializerOptions), Encoding.UTF8, "application/json");
            }

            using var httpResponseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.Content?.Headers?.ContentType == null)
            {
                return default;
            }

            await using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return await JsonSerializer.DeserializeAsync<TResponse>(responseStream, _serializerOptions, cancellationToken);
        }

        protected abstract IDictionary<string, string> GetDefaultHeaders();
        protected abstract string GetAuthorizationHeader(string authorization);

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
