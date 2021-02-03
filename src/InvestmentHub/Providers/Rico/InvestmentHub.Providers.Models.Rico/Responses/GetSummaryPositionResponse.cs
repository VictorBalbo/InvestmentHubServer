using System.Collections.Generic;

namespace InvestmentHub.Providers.Models.Rico.Responses
{
    public class GetSummaryPositionResponse
    {
        public double TotalValue { get; set; }
        public double TotalInvestedValue { get; set; }
        public IEnumerable<Position> Positions { get; set; }
    }
}
