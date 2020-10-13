using System;

namespace InvestmentHub.Models
{
    public class Asset
    {
        /// <summary>
        /// Id of the asset.
        /// Is defined as a Hash of ProviderName and AssetName
        /// </summary>
        public string Id { get; set; }

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
        /// Date the asset was saved retrieved.
        /// This date does not mean when the asset was created on provider.
        /// </summary>
        public DateTimeOffset StorageDate { get; set; }

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