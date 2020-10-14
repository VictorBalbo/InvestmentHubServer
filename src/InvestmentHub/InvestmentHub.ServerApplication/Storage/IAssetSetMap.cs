using InvestmentHub.Models;
using Take.Elephant;

namespace InvestmentHub.ServerApplication.Storage
{
    internal interface IAssetSetMap : ISetMap<string, Asset>, IQueryableStorage<Asset>
    {
    }
}