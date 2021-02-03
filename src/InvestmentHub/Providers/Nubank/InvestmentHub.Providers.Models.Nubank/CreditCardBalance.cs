namespace InvestmentHub.Providers.Models.Nubank
{
    public class CreditCardBalance
    {
        public int Available { get; set; }
        public int Due { get; set; }
        public int Future { get; set; }
        public int Open { get; set; }
        public int Prepaid { get; set; }
    }
}
