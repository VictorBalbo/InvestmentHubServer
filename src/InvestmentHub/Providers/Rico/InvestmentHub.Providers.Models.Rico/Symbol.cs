using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Rico.Responses
{
    public class Symbol
    {
        [JsonPropertyName("symbol")]
        public string Name { get; set; }

        public string Market { get; set; }
        public string Type { get; set; }
        public bool Leverage { get; set; }
    }
}