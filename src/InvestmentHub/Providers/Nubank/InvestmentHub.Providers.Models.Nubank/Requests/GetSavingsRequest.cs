namespace InvestmentHub.Providers.Models.Nubank.Requests
{
    public class GetSavingsRequest
    {
        public string Query =>
            "{ viewer { savingsAccount { currentSavingsBalance { netAmount} feed { id __typename title detail postDate ... on TransferInEvent { amount originAccount { name }} ... on TransferOutEvent { amount destinationAccount { name }} ... on DebitPurchaseEvent { amount } ... on BarcodePaymentEvent { amount }}}}}";
    }
}
