using Dawn;
using InvestmentHub.Models;
using InvestmentHub.ServerApplication.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Controllers
{
    [Authorize]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IConfigurations _configurations;

        public AuthenticationController(IAccountManager accountManager, IConfigurations configurations)
            : base()
        {
            _accountManager = accountManager;
            _configurations = configurations;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Account account, CancellationToken cancellationToken)
        {
            Guard.Argument(account.Email, nameof(account.Email)).NotNull();
            Guard.Argument(account.Password, nameof(account.Password)).NotNull();

            var storageAccount = await _accountManager.CreateAccountAsync(account, cancellationToken);
            if (storageAccount == null)
            {
                throw new Exception("Could not save the account");
            }

            string tokenString = GetAuthToken(storageAccount);

            // return basic user info and authentication token
            return Ok(new
            {
                account.Email,
                account.Name,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Account account, CancellationToken cancellationToken)
        {
            Guard.Argument(account.Email, nameof(account.Email)).NotNull();
            Guard.Argument(account.Password, nameof(account.Password)).NotNull();

            var storageAccount = await _accountManager.AuthenticateAsync(account, cancellationToken);
            if (storageAccount == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            string tokenString = GetAuthToken(account);

            // return basic user info and authentication token
            return Ok(new
            {
                account.Email,
                account.Name,
                Token = tokenString
            });
        }

        [HttpPost("logout")]
        public Task<IActionResult> Logout([FromBody]Account account, CancellationToken cancellationToken)
        {
            ///TODO: Implement logout
            throw new NotImplementedException();
        }

        private string GetAuthToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configurations.SymmetricKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}