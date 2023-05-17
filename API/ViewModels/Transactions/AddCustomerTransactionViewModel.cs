using BankApplicationModels.Enums;

namespace API.ViewModels.Transactions
{
    public class AddCustomerTransactionViewModel
    {
        public string? CustomerBankId { get; set; }
        public string? CustomerBranchId { get; set; }
        public string? CustomerAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? TransactionDate { get; set; }
        public decimal Balance { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
