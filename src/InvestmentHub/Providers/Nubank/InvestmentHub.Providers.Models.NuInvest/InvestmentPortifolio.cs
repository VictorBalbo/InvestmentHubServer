using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.NuInvest
{
    public class InvestmentPortifolio
    {
        [JsonPropertyName("investmentType")]
        public InvestmentType InvestmentType { get; set; }

        [JsonPropertyName("isDelayed")]
        public bool IsDelayed { get; set; }

        [JsonPropertyName("percentagePortfolio")]
        public double PercentagePortfolio { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
}
