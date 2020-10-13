using InvestmentHub.Models;
using Take.Elephant;

namespace InvestmentHub.ServerApplication.Storage
{
    public interface IAssetSetMap : ISetMap<string, Asset>, IQueryableStorage<Asset>
    {
    }
}