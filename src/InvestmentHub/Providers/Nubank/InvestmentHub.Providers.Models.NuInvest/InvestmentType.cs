using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.NuInvest
{
    public class InvestmentType
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
