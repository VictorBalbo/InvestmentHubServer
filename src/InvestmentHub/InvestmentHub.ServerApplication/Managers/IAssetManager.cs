using InvestmentHub.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Managers
{
    public interface IAssetManager
    {
        /// <summary>
        /// Fetch each provider for an <paramref name="identity"/> for the registered assets
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="password"></param>
        /// <param name="forceUpdate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> FetchAllProvidersAssets(string identity, string password, bool forceUpdate, CancellationToken cancellationToken);
        
        Task<bool> FetchProviderAssets(ProviderCredentials accountProvider, string password, string code, CancellationToken cancellationToken);

        Task<IAsyncEnumerable<Asset>> GetAllAssetsAsync(string identity, CancellationToken cancellationToken);

        Task<Asset> GetAssetAsync(string identity, string assetId, CancellationToken cancellationToken);
        
        Task<IEnumerable<Asset>> GetCurrentAssetsAsync(string identity, CancellationToken cancellationToken);

        Task SetAssetAsync(string identity, Asset asset, CancellationToken cancellationToken);

        Task<bool> DeleteAssetAsync(string identity, Asset asset, CancellationToken cancellationToken);
    }
}
