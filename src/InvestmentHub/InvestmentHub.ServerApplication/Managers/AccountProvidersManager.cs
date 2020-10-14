using InvestmentHub.Models;
using InvestmentHub.ServerApplication.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Take.Elephant;

namespace InvestmentHub.ServerApplication.Managers
{
    internal class AccountProvidersManager : IAccountProvidersManager
    {
        private readonly IAccountProvidersSetMap _accountProvidersSetMap;

        public AccountProvidersManager(IAccountProvidersSetMap accountProvidersSetMap)
        {
            _accountProvidersSetMap = accountProvidersSetMap;
        }

        public async Task<IAsyncEnumerable<ProviderCredentials>> GetAccountProviderCredentials(string identity, CancellationToken cancellationToken)
        {
            var accountProviders = await _accountProvidersSetMap.GetValueOrEmptyAsync(identity, cancellationToken);
            return accountProviders
                .AsEnumerableAsync()
                .Select(a => a.RemoveSensitiveInformation());
        }

        public async Task SetAccountProviderCredentials(string identity, ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            await _accountProvidersSetMap.AddItemAsync(identity, providerCredentials, cancellationToken);
        }

        public async Task<bool> DeleteAccountProviderCredentials(string identity, ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            return await _accountProvidersSetMap.TryRemoveItemAsync(identity, providerCredentials, cancellationToken);
        }
    }
}