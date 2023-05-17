using BankApplicationModels.Enums;

namespace API.Models
{
    public class TransactionDto
    {
        public string? CustomerAccountId { get; set; }
        public string? CustomerBankId { get; set; }
        public string? CustomerBranchId { get; set; }
        public string? FromCustomerBankId { get; set; }

        public string? ToCustomerBankId { get; set; }

        public string? FromCustomerBranchId { get; set; }

        public string? ToCustomerBranchId { get; set; }

        public string? TransactionId { get; set; }

        public string? FromCustomerAccountId { get; set; }

        public TransactionType TransactionType { get; set; }

        public string? ToCustomerAccountId { get; set; }

        public string? TransactionDate { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public decimal Balance { get; set; }
    }
}
