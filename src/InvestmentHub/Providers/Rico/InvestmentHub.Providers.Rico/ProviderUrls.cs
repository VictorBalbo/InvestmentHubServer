namespace InvestmentHub.Providers.Rico
{
    internal class ProviderUrls
    {
        /// <summary>
        /// First step of authentication. Used to get the password keyboard
        /// </summary>
        public const string GET_KEYBOARD = "https://www.rico.com.vc/api/oauth/keyboard/";

        /// <summary>
        /// Second step of authentication. Used to finish authentication
        /// </summary>
        public const string AUTHENTICATE = "https://www.rico.com.vc/api/oauth/";

        /// <summary>
        /// Get customer informations.
        /// </summary>
        public const string GET_CUSTOMER_INFORMATION = "https://www.rico.com.vc/api/customer-info";

        /// <summary>
        /// Get more information on actives of type stock
        /// </summary>
        public const string GET_SUMMARY_POSITION = "https://www.rico.com.vc/api/finance/summary-position/";

        /// <summary>
        /// Get more information on actives of type stock
        /// </summary>
        public const string GET_POSITION_DETAILS = "https://www.rico.com.vc/api/exchange/positions/?type={TYPE}";
    }
}