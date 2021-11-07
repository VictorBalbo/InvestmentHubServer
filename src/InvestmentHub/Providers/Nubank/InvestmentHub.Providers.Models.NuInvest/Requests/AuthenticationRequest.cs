using System;
using System.Collections.Generic;

namespace InvestmentHub.Providers.Models.NuInvest.Requests
{
    public class AuthenticationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OtpCode { get; set; }
        public string DeviceUid { get; set; }

        public const string GrantType = "password";
        public const string ClientId = "876dab2190464884bf9b092aa1407585";

        public AuthenticationRequest(string userName, string password, string code = null)
        {
            Username = Uri.EscapeDataString(userName);
            Password = Uri.EscapeDataString(password);
            OtpCode = code;
            DeviceUid = Guid.NewGuid().ToString("N");
        }

        public string BuildFormValues()
        {
            return $"username={Username}&password={Password}&grant_type={GrantType}&client_id={ClientId}&device_uid={DeviceUid}&otp_code={OtpCode}";
        }

        public Dictionary<string, string> GetHeaders()
        {
            return new Dictionary<string, string>
            {
                { "content-type", "application/x-www-form-urlencoded;charset=UTF-8" },
            };
        }
    }
}
