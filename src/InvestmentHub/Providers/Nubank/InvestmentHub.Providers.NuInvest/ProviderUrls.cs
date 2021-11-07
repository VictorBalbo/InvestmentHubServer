namespace InvestmentHub.Providers.NuInvest
{
    internal class ProviderUrls
    {
        /// <summary>
        /// Used to authenticate on Nuinvest
        /// </summary>
        internal const string AUTHENTICATE = "https://www.nuinvest.com.br/api/auth/v3/security/token";

        /// <summary>
        /// Used to get details from provider wallet
        /// </summary>
        internal const string GET_POSITION = "https://www.nuinvest.com.br/api/samwise/v2/custody-position";
    }
}
