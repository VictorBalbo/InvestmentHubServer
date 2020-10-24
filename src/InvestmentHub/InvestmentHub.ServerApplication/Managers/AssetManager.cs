using Dawn;
using InvestmentHub.Models;
using InvestmentHub.ServerApplication.Providers;
using InvestmentHub.ServerApplication.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Take.Elephant;

namespace InvestmentHub.ServerApplication.Managers
{
    internal class AssetManager : IAssetManager
    {
        private readonly IAssetSetMap _assetSetMap;
        private readonly IProviderContainer _providerContainer;
        private readonly IEncryptorManager _encryptorManager;
        private readonly IAccountProvidersManager _accountProvidersManager;

        public AssetManager(IAssetSetMap assetSetMap,
            IProviderContainer providerContainer,
            IEncryptorManager encryptorManager,
            IAccountProvidersManager accountProvidersManager)
        {
            _assetSetMap = assetSetMap;
            _providerContainer = providerContainer;
            _encryptorManager = encryptorManager;
            _accountProvidersManager = accountProvidersManager;
        }

        public async Task<bool> GetProviderAssets(string identity, string password, CancellationToken cancellationToken)
        {
            Guard.Argument(identity).NotNull().NotEmpty();
            Guard.Argument(password).NotNull().NotEmpty();

            var accountProviders = await _accountProvidersManager.GetAccountProviderCredentials(identity, cancellationToken);
            try
            {
                await foreach (var account in accountProviders)
                {
                    var providerUserName = _encryptorManager.Decrypt(account.ProviderUserName, password);
                    var providerUserPassword = _encryptorManager.Decrypt(account.ProviderUserPassword, password);

                    var provider = _providerContainer.GetProvider(account.ProviderName);
                    var isLoginSuccessful = await provider.LoginAsync(providerUserName, providerUserPassword, cancellationToken);
                    if (isLoginSuccessful)
                    {
                        var assets = await provider.GetAssetsAsync(cancellationToken);
                        var saveAssetsTask = assets.Select(a => SetAssetAsync(account.Email, a, cancellationToken));
                        await Task.WhenAll(saveAssetsTask);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Asset> GetAssetAsync(string identity, string assetId, CancellationToken cancellationToken)
        {
            var set = await _assetSetMap.GetValueOrDefaultAsync(identity, cancellationToken);
            if (set == null) return default;

            var queryable = set as IQueryableStorage<Asset>;
            var result = await queryable.QueryAsync(a => a.Id == assetId, a => a, 0, 1, cancellationToken);

            return await result.Items.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IAsyncEnumerable<Asset>> GetAssetsAsync(string identity, CancellationToken cancellationToken)
        {
            var set = await _assetSetMap.GetValueOrDefaultAsync(identity, cancellationToken);
            return set?.AsEnumerableAsync();
        }

        public async Task SetAssetAsync(string identity, Asset asset, CancellationToken cancellationToken)
        {
            await _assetSetMap.AddItemAsync(identity, asset, cancellationToken);
        }

        public async Task<bool> DeleteAssetAsync(string identity, Asset asset, CancellationToken cancellationToken)
        {
            return await _assetSetMap.TryRemoveItemAsync(identity, asset, cancellationToken);
        }
    }
}
