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
    [Route(UriTemplates.ASSETS)]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetManager _assetManager;

        public AssetsController(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        [HttpGet]
        public async Task<IAsyncEnumerable<Asset>> GetOwnAssets(CancellationToken cancellationToken)
        {
            var assets = await _assetManager.GetAssetsAsync(User.Identity.Name, cancellationToken);
            return assets;
        }

        [HttpPost]
        public async Task FetchOwnAssets([FromBody]string password, CancellationToken cancellationToken)
        {
            await _assetManager.GetProviderAssets(User.Identity.Name, password, true, cancellationToken);
        }
    }
}
