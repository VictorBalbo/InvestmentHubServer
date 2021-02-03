using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Nubank.Responses
{
    public class GetSavingsResponse : BaseResponse
    {
        public DataResponse Data { get; set; }

        [JsonIgnore]
        public IEnumerable<Saving> Savings => Data.Viewer.SavingsAccount.Feed;
        
        [JsonIgnore]
        public double NetAmount => Data.Viewer.SavingsAccount.CurrentSavingsBalance.NetAmount;
    }

    public class DataResponse
    {
        public ViewerResponse Viewer { get; set; }
    }

    public class ViewerResponse
    {
        public SavingsAccount SavingsAccount { get; set; }
    }

    public class SavingsAccount
    {
        public IEnumerable<Saving> Feed { get; set; }
        
        public CurrentSavingsBalance CurrentSavingsBalance { get; set; }
    }
    
    public class CurrentSavingsBalance
    {
        public double NetAmount { get; set; }
    }
}
