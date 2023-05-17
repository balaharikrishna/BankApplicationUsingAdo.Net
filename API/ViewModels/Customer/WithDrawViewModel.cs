namespace API.ViewModels.Customer
{
    public class WithDrawViewModel
    {
        public string? BankId { get; set; }
        public string? BranchId { get; set; }
        public string? AccountId { get; set; }
        public decimal withDrawAmount { get; set; }
    }
}
