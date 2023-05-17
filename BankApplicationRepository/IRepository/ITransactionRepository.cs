using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface ITransactionRepository
    {
        Task<bool> AddTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllTransactions(string accountId);
        Task<Transaction?> GetTransactionById(string accountId, string transactionId);
        Task<bool> IsTransactionsExist(string accountId);
    }
}