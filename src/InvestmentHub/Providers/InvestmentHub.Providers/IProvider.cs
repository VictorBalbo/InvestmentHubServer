using InvestmentHub.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.Providers
{
    public interface IProvider
    {
        string ProviderName { get; }

        Task<bool> LoginAsync(string userName, string userPassword, CancellationToken cancellationToken);

        Task<IEnumerable<Asset>> GetAssetsAsync(CancellationToken cancellationToken);
    }
}