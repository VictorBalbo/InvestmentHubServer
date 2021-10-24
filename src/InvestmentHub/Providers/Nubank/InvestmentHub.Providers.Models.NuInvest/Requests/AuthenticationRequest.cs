using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.NuInvest.Requests
{
    public class AuthenticationRequest
    {
        public const string UserNameSelector = "#username";
        public const string PasswordSelector = "#password";
        public const string EnterKey = "Enter";
    }
}
