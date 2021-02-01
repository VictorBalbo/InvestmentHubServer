using InvestmentHub.ServerApplication.Managers;
using InvestmentHub.ServerApplication.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Services
{
    public class AssetUpdateService : BaseService, IAssetUpdateService
    {
        private readonly IAssetManager _assetManager;
        private readonly IPasswordMap _passwordMap;
        private readonly IConfigurations _configurations;
        private readonly IEncryptorManager _encryptorManager;

        public AssetUpdateService(IAssetManager assetManager, IPasswordMap passwordMap, IConfigurations configurations, IEncryptorManager encryptorManager)
        {
            _assetManager = assetManager;
            _passwordMap = passwordMap;
            _configurations = configurations;
            _encryptorManager = encryptorManager;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cachedAccountIds = await _passwordMap.GetKeysAsync();
                    await foreach (var accountId in cachedAccountIds.WithCancellation(cancellationToken))
                    {
                        var encryptedPassword = await _passwordMap.GetValueOrDefaultAsync(accountId, cancellationToken);
                        var accountPassword = _encryptorManager.Decrypt(encryptedPassword, _configurations.SymmetricKey);
                        await _assetManager.FetchProviderAssets(accountId, accountPassword, false, cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

                await Task.Delay(TimeSpan.FromDays(1), cancellationToken);
            }
        }
    }
}
