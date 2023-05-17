using BankApplicationModels;
using BankApplicationModels.Enums;

namespace BankApplicationServices.IServices
{
    public interface ITransactionService
    {
        Task<Transaction> GetTransactionById(string accountId, string transactionId);
        /// <summary>
        /// Checks if transactions are available for the specified customer account.
        /// </summary>
        /// <param name="fromBankId">The bank ID of the source account.</param>
        /// <param name="fromBranchId">The branch ID of the source account.</param>
        /// <param name="fromCustomerAccountId">The customer account ID of the source account.</param>
        /// <returns>A message indicating status of Transactions Availability.</returns>
        Task<Message> IsTransactionsAvailableAsync(string accountId);

        /// <summary>
        /// Retrieves the transaction history for the specified customer account.
        /// </summary>
        /// <param name="fromCustomerAccountId">The customer account ID of the source account.</param>
        /// <returns>A list of strings representing the transaction history.</returns>
        Task<IEnumerable<Transaction>> GetAllTransactionHistory(string accountId);

        /// <summary>
        /// Reverts a transaction with the specified ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to be reverted.</param>
        /// <param name="fromBankId">The bank ID of the source account.</param>
        /// <param name="fromBranchId">The branch ID of the source account.</param>
        /// <param name="fromCustomerAccountId">The customer account ID of the source account.</param>
        /// <param name="toBankId">The bank ID of the destination account.</param>
        /// <param name="toBranchId">The branch ID of the destination account.</param>
        /// <param name="toCustomerAccountId">The customer account ID of the destination account.</param>
        /// <returns>A message indicating status to Revert the Transaction.</returns>
        Task<Message> RevertTransactionAsync(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId);

        /// <summary>
        /// Logs transaction history for an account with debit or credit information.
        /// </summary>
        /// <param name="fromBankId">The bank ID of the account being debited or credited.</param>
        /// <param name="fromBranchId">The branch ID of the account being debited or credited.</param>
        /// <param name="fromCustomerAccountId">The account ID of the account being debited or credited.</param>
        /// <param name="debitAmount">The amount of money being debited from the account.</param>
        /// <param name="creditAmount">The amount of money being credited to the account.</param>
        /// <param name="fromCustomerbalance">The remaining balance in the account after the transaction is completed.</param>
        /// <param name="transactionType">An integer code indicating the type of transaction being logged.</param>
        Task<Message> TransactionHistoryAsync(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount,
          decimal creditAmount, decimal fromCustomerbalance, TransactionType transactionType);

        /// <summary>
        /// Logs transaction history for From and To Customer accounts with both debit and credit information.
        /// </summary>
        /// <param name="fromBankId">The bank ID of the account being debited.</param>
        /// <param name="fromBranchId">The branch ID of the account being debited.</param>
        /// <param name="fromCustomerAccountId">The account ID of the account being debited.</param>
        /// <param name="toBankId">The bank ID of the account being credited.</param>
        /// <param name="toBranchId">The branch ID of the account being credited.</param>
        /// <param name="toCustomerAccountId">The account ID of the account being credited.</param>
        /// <param name="debitAmount">The amount of money being debited from the account.</param>
        /// <param name="creditAmount">The amount of money being credited to the account.</param>
        /// <param name="fromCustomerbalance">The remaining balance in the debited account after the transaction is completed.</param>
        /// <param name="toCustomerBalance">The remaining balance in the credited account after the transaction is completed.</param>
        /// <param name="transactionType">An integer code indicating the type of transaction being logged.</param>
        Task<Message> TransactionHistoryFromAndToAsync(string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId,
            decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, decimal toCustomerBalance, TransactionType transactionType);
    }
}

