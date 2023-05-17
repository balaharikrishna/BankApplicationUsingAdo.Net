namespace API.ViewModels.Transactions
{
    public class RevertTransactionViewModel
    {

        public string? TransactionId { get; set; }
        public string? FromCustomerBankId { get; set; }
        public string? FromCustomerAccountId { get; set; }
        public string? FromCustomerBranchId { get; set; }
        public string? ToCustomerBankId { get; set; }
        public string? ToCustomerAccountId { get; set; }
        public string? ToCustomerBranchId { get; set; }
    }
}
