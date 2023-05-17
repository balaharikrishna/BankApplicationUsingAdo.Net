using BankApplicationModels.Enums;

namespace API.ViewModels.Customer
{
    public class TransferAmountViewModel
    {
        public string? BankId { get; set; }
        public string? ToBankId { get; set; }
        public string? BranchId { get; set; }
        public string? ToBranchId { get; set; }
        public string? AccountId { get; set; }
        public string? ToAccountId { get; set; }
        public decimal TransferAmount { get; set; }
        public TransferMethod TransferMethod { get; set; }
    }
}
