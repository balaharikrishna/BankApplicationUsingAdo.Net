using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Transaction
    {
        [Required]
        public string CustomerAccountId { get; set; }
        [Required]
        public string CustomerBankId { get; set; }
        [Required]
        public string CustomerBranchId { get; set; }
        public string FromCustomerBankId { get; set; }
        public string ToCustomerBankId { get; set; }
        public string FromCustomerBranchId { get; set; }
        public string ToCustomerBranchId { get; set; }
        public string FromCustomerAccountId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        public string ToCustomerAccountId { get; set; }
        [Required]
        public string TransactionDate { get; set; }
        [Required]
        public decimal Debit { get; set; }
        [Required]
        public decimal Credit { get; set; }
        [Required]
        public decimal Balance { get; set; }

        //public override string ToString()
        //{
        //    return $"{TransactionId}: {TransactionType} " +
        // $"From BankId:{FromCustomerBankId}-BranchId:{FromCustomerBranchId}-AccountId:{FromCustomerAccountId} " +
        // $"To BankId:{ToCustomerBankId}-BranchId:{ToCustomerBranchId}-AccountId:{ToCustomerAccountId} " +
        // $"on {TransactionDate}: Debited Amount:{Debit}, Credited Amount:{Credit}, Balance:{Balance}";
        //}
    }
}
