using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.NuInvest.Requests
{
    public class Investment
    {
        [JsonPropertyName("custodyId")]
        public int CustodyId { get; set; }

        [JsonPropertyName("grossValue")]
        public double GrossValue { get; set; }

        [JsonPropertyName("investmentType")]
        public InvestmentType InvestmentType { get; set; }

        [JsonPropertyName("nickName")]
        public string NickName { get; set; }

        [JsonPropertyName("rentability")]
        public string Rentability { get; set; }

        [JsonPropertyName("securityNameType")]
        public string SecurityNameType { get; set; }

        [JsonPropertyName("securityType")]
        public string SecurityType { get; set; }

        [JsonPropertyName("marketCode")]
        public string MarketCode { get; set; }

        [JsonPropertyName("stockCode")]
        public string StockCode { get; set; }
    }
}
