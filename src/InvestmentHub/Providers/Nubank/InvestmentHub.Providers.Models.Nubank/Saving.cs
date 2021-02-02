using System;
using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Nubank
{
    public class Saving
    {
        public string Id { get; set; }

        [JsonPropertyName("__typename")]
        public string TypeName { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime PostDate { get; set; }
        public float Amount { get; set; }
        public Account OriginAccount { get; set; }
        public Account DestinationAccount { get; set; }
    }

    public class Account
    {
        public string Name { get; set; }
    }
}
