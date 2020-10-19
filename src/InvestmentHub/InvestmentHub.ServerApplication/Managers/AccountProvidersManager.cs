using Dawn;
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
        private readonly IEncryptorManager _encryptorManager;

        public AccountProvidersManager(IAccountProvidersSetMap accountProvidersSetMap, IEncryptorManager encryptorManager)
        {
            _accountProvidersSetMap = accountProvidersSetMap;
            _encryptorManager = encryptorManager;
        }

        public async Task<IAsyncEnumerable<ProviderCredentials>> GetAccountProviderCredentials(string identity, CancellationToken cancellationToken)
        {
            var accountProviders = await _accountProvidersSetMap.GetValueOrEmptyAsync(identity, cancellationToken);
            return accountProviders
                .AsEnumerableAsync()
                .Select(a =>
                {
                    a.ProviderUserName = _encryptorManager.Decrypt(a.ProviderUserName, "teste123456");
                    a.ProviderUserPassword = _encryptorManager.Decrypt(a.ProviderUserPassword, "teste123456");
                    return a;
                });
        }

        public async Task<IAsyncEnumerable<ProviderCredentials>> GetSecuredAccountProviderCredentials(string identity, CancellationToken cancellationToken)
        {
            var accountProviders = await _accountProvidersSetMap.GetValueOrEmptyAsync(identity, cancellationToken);
            return accountProviders
                .AsEnumerableAsync()
                .Select(a => a.RemoveSensitiveInformation());
        }

        public async Task SetAccountProviderCredentials(string identity, string password, ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            Guard.Argument(identity).NotNull().NotWhiteSpace();
            Guard.Argument(password).NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials).NotNull();
            Guard.Argument(providerCredentials.ProviderName, "ProviderName").NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.Email, "Email").NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.ProviderUserName, "ProviderUserName").NotNull().NotWhiteSpace();
            Guard.Argument(providerCredentials.ProviderUserPassword, "ProviderUserPassword").NotNull().NotWhiteSpace();

            providerCredentials.ProviderUserName = _encryptorManager.Encrypt(providerCredentials.ProviderUserName, password);
            providerCredentials.ProviderUserPassword = _encryptorManager.Encrypt(providerCredentials.ProviderUserPassword, password);

            await _accountProvidersSetMap.AddItemAsync(identity, providerCredentials, cancellationToken);
        }

        public async Task<bool> DeleteAccountProviderCredentials(string identity, ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            return await _accountProvidersSetMap.TryRemoveItemAsync(identity, providerCredentials, cancellationToken);
        }
    }
}
