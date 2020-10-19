using System;
using System.Collections.Generic;
using InvestmentHub.Providers;

namespace InvestmentHub.ServerApplication.Providers
{
    public class ProviderContainer : IProviderContainer
    {
        private readonly IDictionary<string, IProvider> _providerDictionary;

        public ProviderContainer(IEnumerable<IProvider> providers)
        {
            _providerDictionary = new Dictionary<string, IProvider>();

            foreach (var provider in providers)
            {
                _providerDictionary.Add(provider.ProviderName, provider);
            }
        }

        public IProvider GetProvider(string providerName)
        {
            if (!_providerDictionary.TryGetValue(providerName, out var provider))
            {
                throw new ArgumentException($"Provider '{providerName}' not found");
            }
            return provider;
        }
    }
}
