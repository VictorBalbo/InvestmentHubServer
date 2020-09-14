namespace InvestmentHub.Providers.Models.Rico.Requests
{
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string SessionId { get; set; }
        public string OtpToken { get; set; }
    }
}