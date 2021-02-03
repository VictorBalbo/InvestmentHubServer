using System.Collections.Generic;

namespace InvestmentHub.Providers.Models.Rico.Responses
{
    public class GetPositionDetailResponse
    {
        public double CurrentTotalValue { get; set; }
        public IEnumerable<PositionDetail> Positions { get; set; }
    }
}
