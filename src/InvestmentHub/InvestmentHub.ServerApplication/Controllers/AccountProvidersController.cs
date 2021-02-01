using System;
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
    [Route(UriTemplates.ACCOUNTS_PROVIDERS)]
    public class AccountProvidersController : ControllerBase
    {
        private readonly IAccountProvidersManager _accountProvidersManager;

        public AccountProvidersController(IAccountProvidersManager accountProvidersManager)
        {
            _accountProvidersManager = accountProvidersManager;
        }

        [HttpGet]
        public async Task<IAsyncEnumerable<ProviderCredentials>> GetOwnProvidersCredentials(CancellationToken cancellationToken)
        {
            return await _accountProvidersManager.GetSecuredAccountProviderCredentials(User.Identity.Name, cancellationToken);
        }

        [HttpPut]
        [HttpPost]
        public async Task SetOwnProvidersCredentials([FromBody]ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            if (providerCredentials.Email != User.Identity.Name)
            {
                throw new ArgumentException("Account email is not your own");
            }

            await _accountProvidersManager.SetAccountProviderCredentials(User.Identity.Name, providerCredentials.Password, providerCredentials, cancellationToken);
        }

        [HttpDelete]
        public async Task DeleteOwnProvidersCredentials([FromBody]ProviderCredentials providerCredentials, CancellationToken cancellationToken)
        {
            var isDeleted = await _accountProvidersManager.DeleteAccountProviderCredentials(User.Identity.Name, providerCredentials, cancellationToken);
            if (!isDeleted)
            {
                throw new ArgumentException("Could not delete the account provider");
            }
        }
    }
}
