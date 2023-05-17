namespace API.ViewModels.Currency
{
    public class CurrencyViewModel
    {
        public string? BankId { get; set; }
        public string? CurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
