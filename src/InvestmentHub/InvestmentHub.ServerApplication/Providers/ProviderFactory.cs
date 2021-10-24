using System;
using InvestmentHub.Providers;
using InvestmentHub.Providers.Nubank;
using InvestmentHub.Providers.NuInvest;
using InvestmentHub.Providers.Rico;

namespace InvestmentHub.ServerApplication.Providers
{
    public class ProviderFactory : IProviderFactory
    {
        public IProvider GetProvider(string providerName)
        {
            return providerName switch
            {
                RicoProvider.ProviderName => new RicoProvider(),
                NubankProvider.ProviderName => new NubankProvider(),
                NuInvestProvider.ProviderName => new NuInvestProvider(),
                _ => throw new ArgumentException($"Provider '{providerName}' not found")
            };
        }
    }
}
