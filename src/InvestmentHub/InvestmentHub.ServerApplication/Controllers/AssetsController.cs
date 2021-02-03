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
        private readonly IAccountProvidersManager _accountProvidersManager;

        public AssetsController(IAssetManager assetManager, IAccountProvidersManager accountProvidersManager)
        {
            _assetManager = assetManager;
            _accountProvidersManager = accountProvidersManager;
        }
        
        [Route(UriTemplates.ASSET)]
        [HttpGet]
        public async Task<Asset> GetOwnAsset(string assetId, CancellationToken cancellationToken)
        {
            var assets = await _assetManager.GetAssetAsync(User.Identity?.Name, assetId, cancellationToken);
            return assets;
        }

        [Route(UriTemplates.ASSETS)]
        [HttpGet]
        public async Task<IAsyncEnumerable<Asset>> GetOwnAssets(CancellationToken cancellationToken)
        {
            var assets = await _assetManager.GetAllAssetsAsync(User.Identity?.Name, cancellationToken);
            return assets;
        }
        
        [Route(UriTemplates.ASSETS_CURRENT)]
        [HttpGet]
        public async Task<IEnumerable<Asset>> GetOwnCurrentAssets(CancellationToken cancellationToken)
        {
            var assets = await _assetManager.GetCurrentAssetsAsync(User.Identity?.Name, cancellationToken);
            return assets;
        }
        
        [Route(UriTemplates.ASSETS)]
        [HttpPost]
        public async Task FetchOwnProviderForAssets([FromBody]UpdateAssetRequest request, CancellationToken cancellationToken)
        {
            var accountProvider = await _accountProvidersManager.GetAccountProviderCredential(User.Identity?.Name, request.ProviderName,
                cancellationToken);
            await _assetManager.FetchProviderAssets(accountProvider, request.UserPassword, request.SecureCode, cancellationToken);
        }
    }
}
