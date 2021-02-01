using System;

namespace InvestmentHub.Models
{
    public class ProviderCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProviderName { get; set; }
        public string ProviderUserName { get; set; }
        public string ProviderUserPassword { get; set; }
        public bool ShouldCachePassword { get; set; }
        public DateTimeOffset LastSuccessfulUpdate { get; set; }
    }

    public static class ProviderCredentialsExtensions
    {
        public static ProviderCredentials RemoveSensitiveInformation(this ProviderCredentials providerCredentials)
        {
            providerCredentials.Password = null;
            providerCredentials.ProviderUserName = null;
            providerCredentials.ProviderUserPassword = null;
            return providerCredentials;
        }
    }
}
