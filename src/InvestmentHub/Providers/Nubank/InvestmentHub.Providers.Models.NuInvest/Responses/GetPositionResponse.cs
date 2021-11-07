using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.NuInvest.Requests
{
    public class GetPositionResponse
    {
        [JsonPropertyName("availableWithdrawMoney")]
        public double AvailableWithdrawMoney { get; set; }

        [JsonPropertyName("cacheUpdatedData")]
        public DateTime CacheUpdatedData { get; set; }

        [JsonPropertyName("delayedMessages")]
        public string DelayedMessages { get; set; }

        [JsonPropertyName("easyBalance")]
        public double EasyBalance { get; set; }

        [JsonPropertyName("hasEquity")]
        public bool HasEquity { get; set; }

        [JsonPropertyName("hasIpo")]
        public bool HasIpo { get; set; }

        [JsonPropertyName("investmentPortfolio")]
        public IEnumerable<InvestmentPortifolio> InvestmentPortfolio { get; set; }

        [JsonPropertyName("investments")]
        public IEnumerable<Investment> Investments { get; set; }

        [JsonPropertyName("investmentsQuantity")]
        public double InvestmentsQuantity { get; set; }

        [JsonPropertyName("isCached")]
        public bool IsCached { get; set; }

        [JsonPropertyName("isDelayed")]
        public bool IsDelayed { get; set; }

        [JsonPropertyName("isFirstInvestment")]
        public bool IsFirstInvestment { get; set; }

        [JsonPropertyName("isProjectedBalance")]
        public bool IsProjectedBalance { get; set; }

        [JsonPropertyName("totalBalance")]
        public double TotalBalance { get; set; }

        [JsonPropertyName("totalInvestments")]
        public double TotalInvestments { get; set; }
    }
}
