using InvestmentHub.Models;
using InvestmentHub.ServerApplication.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route(UriTemplates.ACCOUNTS)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpGet]
        public async Task<Account> GetOwnAccount(CancellationToken cancellationToken)
        {
            var account = await _accountManager.GetAccountAsync(User.Identity.Name, cancellationToken);
            return account;
        }

        [HttpPost]
        [HttpPut]
        public async Task SetOwnAccount([FromBody]Account account, CancellationToken cancellationToken)
        {
            if (account.Email != User.Identity.Name)
            {
                throw new ArgumentException("Account email is not your own");
            }

            var isUpdated = await _accountManager.UpdateAccountAsync(account, cancellationToken);
            if (!isUpdated)
            {
                throw new Exception("Could not save the account");
            }
        }

        [HttpDelete]
        public async Task DeleteOwnAccount(CancellationToken cancellationToken)
        {
            var isDeleted = await _accountManager.DeleteAccountAsync(User.Identity.Name, cancellationToken);

            if (!isDeleted)
            {
                throw new ArgumentException("Could not delete the account");
            }
        }
    }
}
