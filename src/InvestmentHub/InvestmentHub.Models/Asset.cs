namespace InvestmentHub.Models
{
    public class Asset
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Name of the investment
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Value in the investment
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Type of the investment
        /// </summary>
        public AssetType Type { get; set; }

        /// <summary>
        /// Is this investment profitable (does it generate passive income)
        /// </summary>
        public bool GeneratesIncome { get; set; }

        /// <summary>
        /// Percentage of the investment over the total
        /// </summary>
        public float Alocation { get; set; }

        /// <summary>
        /// Percentage of the investment over the total that generates income
        /// </summary>
        public float InvestedAlocation { get; set; }
    }
}