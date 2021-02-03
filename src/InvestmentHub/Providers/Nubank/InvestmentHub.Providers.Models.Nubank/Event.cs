using System;

namespace InvestmentHub.Providers.Models.Nubank
{
    public class Event
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public double AmountCurrency => Amount / 100.0;
        public DateTimeOffset Time { get; set; }
        public string Title { get; set; }
    }
}
