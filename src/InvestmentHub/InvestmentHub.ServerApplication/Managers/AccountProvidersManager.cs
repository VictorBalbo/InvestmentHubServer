using Dawn;
using InvestmentHub.Models;
using InvestmentHub.ServerApplication.Storage;
using System;
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
        private readonly IEncryptorManager _encryptorManager;
        private readonly IPasswordMap _passwordMap;
        private readonly IConfigurations _configurations;

        public AccountProvidersManager(IAccountProvidersSetMap accountProvidersSetMap, IEncryptorManager encryptorManager, IPasswordMap passwordMap, IConfigurations configurations)
        {
            _accountProvidersSetMap = accountProvidersSetMap;
            _encryptorManager = encryptorManager;
            _passwordMap = passwordMap;
            _configurations = configurations;
        }

        public async Task<IAsyncEnumerable<ProviderCredentials>> GetAccountProviderCredentials(string identity, CancellationToken cancellationToken)
        {
            Guard.Argument(identity).NotNull().NotEmpty();

            var accountProviders = await _accountProvidersSetMap.GetValueOrEmptyAsync(identity, cancellationToken);
            return accountProviders
                .AsEnumerableAsync(cancellationToken);
        }

        public async Task<IAsyncEnumerable<ProviderCredentials>> GetSecuredAccountProviderCredentials(string identity, CancellationToken cancellationToken)
        {
            var accountProviders = await GetAccountProviderCredentials(identity, cancellationToken);
            return accountProviders
                .Select(a => a.RemoveSensitiveInformation());
        }

        public async Task SetAccountProviderCredentials(string identity, string password, ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            Guard.Argument(identity).NotNull().NotWhiteSpace();
            Guard.Argument(password).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials).NotNull();
            Guard.Argument(providerCredentials.ProviderName, nameof(providerCredentials.ProviderName)).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.Email, nameof(providerCredentials.Email)).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.ProviderUserName, nameof(providerCredentials.ProviderUserName)).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.ProviderUserPassword, nameof(providerCredentials.ProviderUserPassword)).NotNull().NotWhiteSpace();

            providerCredentials.ProviderUserName = _encryptorManager.Encrypt(providerCredentials.ProviderUserName, password);
            providerCredentials.ProviderUserPassword = _encryptorManager.Encrypt(providerCredentials.ProviderUserPassword, password);

            await _accountProvidersSetMap.AddItemAsync(identity, providerCredentials, cancellationToken);

            if (providerCredentials.ShouldCachePassword)
            {
                var encryptedPassword = _encryptorManager.Encrypt(password, _configurations.SymmetricKey);
                await _passwordMap.TryAddAsync(identity, encryptedPassword, true, cancellationToken);
            }
        }

        public async Task SetLastSuccessfulUpdate(string identity, ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            Guard.Argument(identity).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials).NotNull();
            Guard.Argument(providerCredentials.ProviderName, nameof(providerCredentials.ProviderName)).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.Email, nameof(providerCredentials.Email)).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.ProviderUserName, nameof(providerCredentials.ProviderUserName)).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.ProviderUserPassword, nameof(providerCredentials.ProviderUserPassword)).NotNull().NotWhiteSpace();

            providerCredentials.LastSuccessfulUpdate = DateTimeOffset.UtcNow;
            await _accountProvidersSetMap.AddItemAsync(identity, providerCredentials, cancellationToken);
        }

        public async Task<bool> DeleteAccountProviderCredentials(string identity, ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            Guard.Argument(identity).NotNull().NotEmpty();

            return await _accountProvidersSetMap.TryRemoveItemAsync(identity, providerCredentials, cancellationToken);
        }
    }
}
