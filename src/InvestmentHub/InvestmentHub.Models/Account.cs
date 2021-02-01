using System;

namespace InvestmentHub.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }

    public static class AccountExtensions
    {
        public static Account RemoveSensitiveInformation(this Account account)
        {
            account.Password = null;
            return account;
        }
    }
}
