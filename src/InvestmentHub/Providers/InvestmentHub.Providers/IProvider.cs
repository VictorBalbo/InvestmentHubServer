using InvestmentHub.Providers.Models;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.Providers
{
    public interface IProvider
    {
        string ProviderName { get; }

        Task<bool> LoginAsync(string userName, string userPassword, CancellationToken cancellationToken);

        Task<InvestmentSummary> GetSavingsAsync(CancellationToken cancellationToken);
    }
}