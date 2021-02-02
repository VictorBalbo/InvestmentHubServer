using System.Text.Json.Serialization;
using InvestmentHub.Providers.Models.Nubank.Urls;

namespace InvestmentHub.Providers.Models.Nubank.Responses
{
    public class AuthenticationResponse : BaseResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        
        [JsonPropertyName("_links")]
        public AuthenticatedUrls Links { get; set; }        
    }
}
