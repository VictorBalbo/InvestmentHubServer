using System.Collections.Generic;

namespace InvestmentHub.Providers.Models.Rico.Responses
{
    public class GetSummaryPositionResponse
    {
        public float TotalValue { get; set; }
        public float TotalInvestedValue { get; set; }
        public IEnumerable<Position> Positions { get; set; }
    }
}