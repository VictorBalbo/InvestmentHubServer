using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InvestmentHub.Models;
using InvestmentHub.ServerApplication.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentHub.ServerApplication.Controllers
{
    [Authorize]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetManager _assetManager;

        public AssetsController(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }
        
        [Route(UriTemplates.ASSET)]
        [HttpGet]
        public async Task<Asset> GetOwnAsset(string assetId, CancellationToken cancellationToken)
        {
            var assets = await _assetManager.GetAssetAsync(User.Identity.Name, assetId, cancellationToken);
            return assets;
        }

        [Route(UriTemplates.ASSETS)]
        [HttpGet]
        public async Task<IAsyncEnumerable<Asset>> GetOwnAssets(CancellationToken cancellationToken)
        {
            var assets = await _assetManager.GetAllAssetsAsync(User.Identity.Name, cancellationToken);
            return assets;
        }
        
        [Route(UriTemplates.ASSETS_CURRENT)]
        [HttpGet]
        public async Task<IEnumerable<Asset>> GetOwnCurrentAssets(CancellationToken cancellationToken)
        {
            var assets = await _assetManager.GetCurrentAssetsAsync(User.Identity.Name, cancellationToken);
            return assets;
        }
        
        [Route(UriTemplates.ASSETS)]
        [HttpPost]
        public async Task FetchOwnAssets([FromBody]string password, CancellationToken cancellationToken)
        {
            await _assetManager.FetchProviderAssets(User.Identity.Name, password, true, cancellationToken);
        }
    }
}
