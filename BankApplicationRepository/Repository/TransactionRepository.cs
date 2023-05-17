using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;

namespace BankApplicationRepository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SqlConnection _connection;
        public TransactionRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactions(string accountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId,CustomerBankId,CustomerBranchId,CustomerAccountId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId,ToCustomerAccountId," +
                "TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId FROM Transactions WHERE CustomerAccountId=@accountId";
            command.Parameters.AddWithValue("@accountId", accountId);
            List<Transaction> transactions = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Transaction transaction = new()
                {
                    TransactionId = reader[0].ToString(),
                    CustomerBankId = reader[1].ToString(),
                    CustomerBranchId = reader[2].ToString(),
                    CustomerAccountId = reader[3].ToString(),
                    FromCustomerBankId = reader[4].ToString(),
                    ToCustomerBankId = reader[5].ToString(),
                    FromCustomerBranchId = reader[6].ToString(),
                    ToCustomerBranchId = reader[7].ToString(),
                    ToCustomerAccountId = reader[8].ToString(),
                    TransactionType = (TransactionType)Convert.ToUInt16(reader[9]),
                    TransactionDate = reader[10].ToString(),
                    Debit = (decimal)reader[11],
                    Credit = (decimal)reader[12],
                    Balance = (decimal)reader[13],
                    FromCustomerAccountId = reader[14].ToString(),
                };
                transactions.Add(transaction);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return transactions;
        }
        public async Task<Transaction?> GetTransactionById(string accountId, string transactionId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId,CustomerBankId,CustomerBranchId,CustomerAccountId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId,ToCustomerAccountId," +
                "TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId FROM Transactions WHERE TransactionId=@TransactionId and CustomerAccountId=@accountId";
            command.Parameters.AddWithValue("@accountId", accountId);
            command.Parameters.AddWithValue("@transactionId", transactionId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                Transaction transaction = new()
                {
                    TransactionId = reader[0].ToString(),
                    CustomerBankId = reader[1].ToString(),
                    CustomerBranchId = reader[2].ToString(),
                    CustomerAccountId = reader[3].ToString(),
                    FromCustomerBankId = reader[4].ToString(),
                    ToCustomerBankId = reader[5].ToString(),
                    FromCustomerBranchId = reader[6].ToString(),
                    ToCustomerBranchId = reader[7].ToString(),
                    ToCustomerAccountId = reader[8].ToString(),
                    TransactionType = (TransactionType)Convert.ToUInt16(reader[9]),
                    TransactionDate = reader[10].ToString(),
                    Debit = (decimal)reader[11],
                    Credit = (decimal)reader[12],
                    Balance = (decimal)reader[13],
                    FromCustomerAccountId = reader[14].ToString(),
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return transaction;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<bool> IsTransactionsExist(string accountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId FROM Transactions WHERE CustomerAccountId=@accountId";
            command.Parameters.AddWithValue("@fromCustomerAccountId", accountId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isTransactionsExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isTransactionsExist;
        }
        public async Task<bool> AddTransaction(Transaction transaction)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Transactions (TransactionId,CustomerAccountId,CustomerBankId,CustomerBranchId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId," +
                "ToCustomerAccountId,TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId )" +
                " VALUES (@transactionId,@customerAccountId,@customerBankId,@customerBranchId,@fromCustomerBankId,@toCustomerBankId,@fromCustomerBranchId,@toCustomerBranchId,@toCustomerAccountId," +
                "@transactionType, @transactionDate,@debit,@credit,@balance,@fromCustomerAccountId)";
            command.Parameters.AddWithValue("@transactionId", transaction.TransactionId);
            command.Parameters.AddWithValue("@customerAccountId", transaction.CustomerAccountId);
            command.Parameters.AddWithValue("@customerBankId", transaction.CustomerBankId);
            command.Parameters.AddWithValue("@customerBranchId", transaction.CustomerBranchId);
            command.Parameters.AddWithValue("@fromCustomerBankId", (object)transaction.FromCustomerBankId ?? DBNull.Value);
            command.Parameters.AddWithValue("@toCustomerBankId", (object)transaction.ToCustomerBankId ?? DBNull.Value);
            command.Parameters.AddWithValue("@fromCustomerBranchId", (object)transaction.FromCustomerBranchId ?? DBNull.Value);
            command.Parameters.AddWithValue("@toCustomerBranchId", (object)transaction.ToCustomerBranchId ?? DBNull.Value);
            command.Parameters.AddWithValue("@toCustomerAccountId", (object)transaction.ToCustomerAccountId ?? DBNull.Value);
            command.Parameters.AddWithValue("@fromCustomerAccountId", (object)transaction.FromCustomerAccountId ?? DBNull.Value);
            command.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("@transactionDate", transaction.TransactionDate);
            command.Parameters.AddWithValue("@debit", transaction.Debit);
            command.Parameters.AddWithValue("@credit", transaction.Credit);
            command.Parameters.AddWithValue("@balance", transaction.Balance);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
    }
}
