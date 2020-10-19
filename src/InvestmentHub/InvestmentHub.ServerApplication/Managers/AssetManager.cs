using InvestmentHub.Models;
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

        public AssetManager(IAssetSetMap assetSetMap)
        {
            _assetSetMap = assetSetMap;
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
