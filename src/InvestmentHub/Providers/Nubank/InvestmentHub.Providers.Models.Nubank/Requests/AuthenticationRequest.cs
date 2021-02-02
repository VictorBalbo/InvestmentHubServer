using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Nubank.Requests
{
    public class AuthenticationRequest
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }
        
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
        
        [JsonPropertyName("login")]
        public string Login { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("qr_code_id")]
        public string QrCodeId { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
