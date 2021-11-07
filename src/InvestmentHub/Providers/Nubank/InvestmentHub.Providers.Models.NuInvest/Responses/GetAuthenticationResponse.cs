using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.NuInvest.Requests
{
    public class GetAuthenticationResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("token_type")]
        public string tokenType { get; set; }
    }
}
