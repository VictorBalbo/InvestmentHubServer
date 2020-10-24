using InvestmentHub.ServerApplication.Managers;
using InvestmentHub.ServerApplication.Providers;
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

        public AssetUpdateService(IAssetManager assetManager, IPasswordMap passwordMap)
        {
            _assetManager = assetManager;
            _passwordMap = passwordMap;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cachedAccountIds = await _passwordMap.GetKeysAsync();
                    await foreach (var accountId in cachedAccountIds)
                    {
                        var accountPass = await _passwordMap.GetValueOrDefaultAsync(accountId, cancellationToken);
                        await _assetManager.GetProviderAssets(accountId, accountPass, cancellationToken);
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
