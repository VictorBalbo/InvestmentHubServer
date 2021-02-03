using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.Providers
{
    /// <summary>
    /// Interface for providers with two steps authentication
    /// </summary>
    public interface ISecureProvider : IProvider
    {
        Task<bool> LoginAsync(string userName, string userPassword, string code, CancellationToken cancellationToken);
    }
}
