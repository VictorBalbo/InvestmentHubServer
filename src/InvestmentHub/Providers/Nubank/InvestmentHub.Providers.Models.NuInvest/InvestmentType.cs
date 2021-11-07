using System.Text.Json.Serialization;
using InvestmentHub.Models;

namespace InvestmentHub.Providers.Models.NuInvest
{
    public class InvestmentType
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("id")]
        public InvestmentTypeEnum Id { get; set; }
    }

    public static class InvestmentTypeExtensions
    {
        public static AssetType GetEquivalentAssetType(this InvestmentType productType)
        {
            switch (productType.Id)
            {
                case InvestmentTypeEnum.FIXED_INCOME:
                case InvestmentTypeEnum.TREASURY:
                    return AssetType.FixedIncome;

                case InvestmentTypeEnum.FUNDS:
                    return AssetType.Fund;

                case InvestmentTypeEnum.STOCK:
                case InvestmentTypeEnum.ETF:
                case InvestmentTypeEnum.FII:
                case InvestmentTypeEnum.BDR:
                    return AssetType.Stock;

                case InvestmentTypeEnum.PENSION_FUNDS:
                    return AssetType.PensionFund;

                case InvestmentTypeEnum.UNKNOWN:
                case InvestmentTypeEnum.VARIABLE_ICOME:
                case InvestmentTypeEnum.COE:
                default:
                    return AssetType.Unknown;
            }
        }
    }

    public enum InvestmentTypeEnum
    {
        TREASURY = 1,
        FIXED_INCOME = 2,
        FUNDS = 3,
        COE = 4,
        PENSION_FUNDS = 6,
        STOCK = 7,
        ETF = 8,
        FII = 9,
        BDR = 10,
        VARIABLE_ICOME = 11,
        UNKNOWN
    }
}
