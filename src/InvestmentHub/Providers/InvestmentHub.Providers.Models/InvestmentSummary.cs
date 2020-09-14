using System.Collections.Generic;

namespace InvestmentHub.Providers.Models
{
    public class InvestmentSummary
    {
        public float TotalValue { get; set; }
        public float TotalInvestedValue { get; set; }
        public IEnumerable<Investment> Investments { get; set; }
    }
}