using InvestmentHub.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Managers
{
    public interface IAssetManager
    {
        Task<bool> GetProviderAssets(string identity, string password, bool forceUpdate, CancellationToken cancellationToken);

        Task<IAsyncEnumerable<Asset>> GetAssetsAsync(string identity, CancellationToken cancellationToken);

        Task<Asset> GetAssetAsync(string identity, string assetId, CancellationToken cancellationToken);

        Task SetAssetAsync(string identity, Asset asset, CancellationToken cancellationToken);

        Task<bool> DeleteAssetAsync(string identity, Asset asset, CancellationToken cancellationToken);
    }
}
