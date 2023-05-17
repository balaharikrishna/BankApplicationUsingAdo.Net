namespace API.ViewModels.Customer
{
    public class DepositViewModel
    {
        public string? BankId { get; set; }
        public string? BranchId { get; set; }
        public string? AccountId { get; set; }
        public decimal DepositAmount { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
