using System.Collections.Generic;

namespace InvestmentHub.Models
{
    public class Wallet
    {
        public string WalletName { get; set; }
        public string AccountIdentity { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
    }
}