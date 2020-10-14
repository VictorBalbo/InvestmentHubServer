using InvestmentHub.Models;
using Take.Elephant;

namespace InvestmentHub.ServerApplication.Storage
{
    /// <summary>
    /// Mapping of an <see cref="Account"/> to a <see cref="ProviderCredentials"/>
    /// </summary>
    internal interface IAccountProvidersSetMap : ISetMap<string, ProviderCredentials>
    {
    }
}