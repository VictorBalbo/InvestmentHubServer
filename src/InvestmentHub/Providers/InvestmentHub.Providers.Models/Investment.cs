namespace InvestmentHub.Providers.Models
{
    public class Investment
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Name of the investment
        /// </summary>
        public string InvestmentName { get; set; }

        /// <summary>
        /// Value in the investment
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Type of the investment
        /// </summary>
        public InvestmentType Type { get; set; }

        /// <summary>
        /// Percentage of the investment over the total
        /// </summary>
        public float Alocation { get; set; }

        /// <summary>
        /// Percentage of the investment over the total that generates income
        /// </summary>
        public float InvestedAlocation { get; set; }

        /// <summary>
        /// Is this investment profitable (does it generate passive income)
        /// </summary>
        public bool GeneratesIncome { get; set; }
    }
}