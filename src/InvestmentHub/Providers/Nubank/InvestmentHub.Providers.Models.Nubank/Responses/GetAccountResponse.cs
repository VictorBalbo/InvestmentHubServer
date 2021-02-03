using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Nubank.Responses
{
    public class GetAccountResponse : BaseResponse
    {
        public NubankAccount Account { get; set; }

        [JsonIgnore]
        public double AvailableCurrency => Account.Balances.Available / 100.0;
        [JsonIgnore]
        public double DueCurrency => Account.Balances.Due / 100.0;
        [JsonIgnore]
        public double FutureCurrency => Account.Balances.Future / 100.0;
        [JsonIgnore]
        public double OpenCurrency => Account.Balances.Open / 100.0;
        [JsonIgnore]
        public double PrepaidCurrency => Account.Balances.Prepaid / 100.0;
    }
}
