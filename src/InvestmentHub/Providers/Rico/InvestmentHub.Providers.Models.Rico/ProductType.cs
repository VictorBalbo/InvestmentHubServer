using InvestmentHub.Models;

namespace InvestmentHub.Providers.Models.Rico
{
    public enum ProductType
    {
        BALANCE,
        OPTION,
        FUTURE,
        STOCK,
        TREASURY,
        FUNDS,
        FIXED_INCOME,
        R8,
        PENSION_FUNDS,
        UNKNOWN
    }

    public static class ProductTypeExtensions
    {
        public static AssetType GetEquivalentAssetType(this ProductType productType)
        {
            switch (productType)
            {
                case ProductType.BALANCE:
                    return AssetType.Balance;

                case ProductType.FIXED_INCOME:
                case ProductType.TREASURY:
                    return AssetType.FixedIncome;

                case ProductType.FUNDS:
                    return AssetType.Fund;

                case ProductType.STOCK:
                    return AssetType.Stock;

                case ProductType.OPTION:
                case ProductType.FUTURE:
                case ProductType.R8:
                case ProductType.PENSION_FUNDS:
                case ProductType.UNKNOWN:
                default:
                    return AssetType.Unknown;
            }
        }
    }
}
