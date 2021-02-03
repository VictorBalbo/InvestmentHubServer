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
        public double Amount { get; set; }
        public NubankAccount OriginAccount { get; set; }
        public NubankAccount DestinationAccount { get; set; }
    }
}
