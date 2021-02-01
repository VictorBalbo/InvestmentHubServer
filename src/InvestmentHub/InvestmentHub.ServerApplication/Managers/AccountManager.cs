using InvestmentHub.Models;
using System.Threading;
using System.Threading.Tasks;
using Dawn;
using InvestmentHub.ServerApplication.Storage;
using BC = BCrypt.Net.BCrypt;
using System;

namespace InvestmentHub.ServerApplication.Managers
{
    internal class AccountManager : IAccountManager
    {
        private readonly IAccountMap _accountMap;

        public AccountManager(IAccountMap accountMap)
        {
            _accountMap = accountMap;
        }

        public async Task<Account> AuthenticateAsync(Account account, CancellationToken cancellationToken)
        {
            Guard.Argument(account.Email, nameof(account.Email)).NotNull();
            Guard.Argument(account.Password, nameof(account.Password)).NotNull();

            var storageAccount = await _accountMap.GetValueOrDefaultAsync(account.Email, cancellationToken);
            if (storageAccount == null)
            {
                return null;
            }

            if (storageAccount.Email != account.Email)
            {
                return null;
            }

            if (!BC.Verify(account.Password, storageAccount.Password))
            {
                return null;
            }
            return storageAccount.RemoveSensitiveInformation();
        }

        public async Task<Account> CreateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            Guard.Argument(account.Email, nameof(account.Email)).NotNull();
            Guard.Argument(account.Password, nameof(account.Password)).NotNull();

            var storageAccount = await _accountMap.GetValueOrDefaultAsync(account.Email, cancellationToken);
            if (storageAccount != null)
            {
                throw new InvalidOperationException("Account already exists");
            }

            account.Password = BC.HashPassword(account.Password);
            account.CreationDate = DateTimeOffset.UtcNow;

            if (await _accountMap.TryAddAsync(account.Email, account, true, cancellationToken))
            {
                return account.RemoveSensitiveInformation();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            Guard.Argument(account.Email, nameof(account.Email)).NotNull();

            var storageAccount = await _accountMap.GetValueOrDefaultAsync(account.Email, cancellationToken);
            account.Password = storageAccount.Password;

            return await _accountMap.TryAddAsync(account.Email, account, true, cancellationToken);
        }

        public async Task<Account> GetAccountAsync(string email, CancellationToken cancellationToken)
        {
            Guard.Argument(email).NotNull();

            var account = await _accountMap.GetValueOrDefaultAsync(email, cancellationToken);
            return account.RemoveSensitiveInformation();
        }

        public async Task<bool> DeleteAccountAsync(string email, CancellationToken cancellationToken)
        {
            Guard.Argument(email).NotNull();

            return await _accountMap.TryRemoveAsync(email, cancellationToken);
        }
    }
}
