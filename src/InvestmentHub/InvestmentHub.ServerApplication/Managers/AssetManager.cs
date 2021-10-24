using Dawn;
using InvestmentHub.Models;
using InvestmentHub.ServerApplication.Providers;
using InvestmentHub.ServerApplication.Storage;
using System;
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
        private readonly IProviderFactory _providerFactory;
        private readonly IEncryptorManager _encryptorManager;
        private readonly IAccountProvidersManager _accountProvidersManager;

        public AssetManager(IAssetSetMap assetSetMap,
            IProviderFactory providerFactory,
            IEncryptorManager encryptorManager,
            IAccountProvidersManager accountProvidersManager)
        {
            _assetSetMap = assetSetMap;
            _providerFactory = providerFactory;
            _encryptorManager = encryptorManager;
            _accountProvidersManager = accountProvidersManager;
        }

        public async Task<bool> FetchAllProvidersAssets(string identity, string password, bool forceUpdate, CancellationToken cancellationToken)
        {
            Guard.Argument(identity).NotNull().NotEmpty();
            Guard.Argument(password).NotNull().NotEmpty();

            var accountProviders = await _accountProvidersManager.GetAccountProviderCredentials(identity, cancellationToken);
            try
            {
                await foreach (var accountProvider in accountProviders.WithCancellation(cancellationToken))
                {
                    if (!forceUpdate && accountProvider.LastSuccessfulUpdate.UtcDateTime > DateTimeOffset.UtcNow.Date)
                    {
                        continue;
                    }

                    await FetchProviderAssets(accountProvider, password, null, cancellationToken);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> FetchProviderAssets(ProviderCredentials accountProvider, string password, string code, CancellationToken cancellationToken)
        {
            var providerUserName = _encryptorManager.Decrypt(accountProvider.ProviderUserName, password);
            var providerUserPassword = _encryptorManager.Decrypt(accountProvider.ProviderUserPassword, password);

            try
            {
                var provider = _providerFactory.GetProvider(accountProvider.ProviderName);
                var isLoginSuccessful = await provider.LoginAsync(providerUserName, providerUserPassword, code, cancellationToken);
                if (!isLoginSuccessful)
                {
                    return false;
                }

                var assets = await provider.GetAssetsAsync(cancellationToken);
                var dateTimeNow = DateTimeOffset.UtcNow;

                if (accountProvider.LastSuccessfulUpdate.Date == dateTimeNow.Date)
                {
                    var sameDayAssets = await GetCurrentAssetsForProviderAsync(accountProvider.Email, accountProvider, cancellationToken);
                    var deleteAssetsTasks =
                        sameDayAssets.Select(a => DeleteAssetAsync(accountProvider.Email, a, cancellationToken));
                    await Task.WhenAll(deleteAssetsTasks);
                }

                var saveAssetsTask = assets.Select(a =>
                {
                    a.StorageDate = dateTimeNow;
                    return SetAssetAsync(accountProvider.Email, a, cancellationToken);
                });
                await Task.WhenAll(saveAssetsTask);
                await _accountProvidersManager.SetLastSuccessfulUpdate(accountProvider.Email, accountProvider, dateTimeNow, cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Asset> GetAssetAsync(string identity, string assetId, CancellationToken cancellationToken)
        {
            var set = await _assetSetMap.GetValueOrDefaultAsync(identity, cancellationToken);

            var queryable = set as IQueryableStorage<Asset>;
            if (queryable == null) return default;
            var result = await queryable.QueryAsync(a => a.Id == assetId, a => a, 0, 1, cancellationToken);

            return await result.Items.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IAsyncEnumerable<Asset>> GetAllAssetsAsync(string identity, CancellationToken cancellationToken)
        {
            var set = await _assetSetMap.GetValueOrDefaultAsync(identity, cancellationToken);
            return set?.AsEnumerableAsync(cancellationToken);
        }

        public async Task<IEnumerable<Asset>> GetCurrentAssetsAsync(string identity, CancellationToken cancellationToken)
        {
            var providers = await _accountProvidersManager.GetSecuredAccountProviderCredentials(identity, cancellationToken);
            if (providers == null)
            {
                return null;
            }

            var assetsTasks = providers.Select((provider) =>
                _assetSetMap.QueryAsync(
                    a =>
                        a.Identity == identity &&
                        a.ProviderName == provider.ProviderName &&
                        a.StorageDate == provider.LastSuccessfulUpdate,
                    a => a,
                    0,
                    1000,
                    cancellationToken));

            var assets = await Task.WhenAll(assetsTasks.ToEnumerable());
            return assets.SelectMany(a => a.Items.ToEnumerable());
        }

        public async Task<IEnumerable<Asset>> GetCurrentAssetsForProviderAsync(string identity, ProviderCredentials provider, CancellationToken cancellationToken)
        {
            Guard.Argument(provider).NotNull();

            var assets = await _assetSetMap.QueryAsync(
                a =>
                    a.Identity == identity &&
                    a.ProviderName == provider.ProviderName &&
                    a.StorageDate == provider.LastSuccessfulUpdate,
                a => a,
                0,
                1000,
                cancellationToken);

            return assets.ToEnumerable();
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
