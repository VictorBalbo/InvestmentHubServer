using System;
using InvestmentHub.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Managers
{
    public interface IAccountProvidersManager
    {
        Task<ProviderCredentials> GetAccountProviderCredential(string identity, string providerName, CancellationToken cancellationToken);

        Task<ProviderCredentials> GetSecuredAccountProviderCredential(string identity, string providerName, CancellationToken cancellationToken);
        
        Task<IAsyncEnumerable<ProviderCredentials>> GetAccountProviderCredentials(string identity, CancellationToken cancellationToken);

        Task<IAsyncEnumerable<ProviderCredentials>> GetSecuredAccountProviderCredentials(string identity, CancellationToken cancellationToken);

        Task SetAccountProviderCredentials(string identity, string password, ProviderCredentials providerCredentials, CancellationToken cancellationToken);

        Task SetLastSuccessfulUpdate(string identity, ProviderCredentials providerCredentials, DateTimeOffset dateTimeOffset, CancellationToken cancellationToken);

        Task<bool> DeleteAccountProviderCredentials(string identity, ProviderCredentials providerCredentials, CancellationToken cancellationToken);
    }
}
