namespace InvestmentHub.Models
{
    public class ProviderCredentials
    {
        public string Email { get; set; }
        public string ProviderName { get; set; }
        public string ProviderUserName { get; set; }
        public string ProviderUserPassword { get; set; }
    }

    public static class ProviderCredentialsExtensions
    {
        public static ProviderCredentials RemoveSensitiveInformation(this ProviderCredentials providerCredentials)
        {
            providerCredentials.ProviderUserPassword = null;
            return providerCredentials;
        }
    }
}
