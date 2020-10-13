using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Rico.Responses
{
    public class GetKeyboardResponse
    {
        public string Token { get; set; }

        [JsonPropertyName("keyboard")]
        public string KeyboardId { get; set; }

        public CustomerInfo Customer { get; set; }
        public IDictionary<string, IEnumerable<string>> Keys { get; set; }
        public int ExpireIn { get; set; }
    }
}