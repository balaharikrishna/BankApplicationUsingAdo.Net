using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Currency
    {
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public decimal ExchangeRate { get; set; }

        public string DefaultCurrencyCode = "INR";

        public short DefaultCurrencyExchangeRate = 1;
        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }
    }
}
