using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class BankRepository : IBankRepository
    {
        private readonly SqlConnection _connection;

        public BankRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BankId, BankName FROM Banks WHERE IsActive = 1";
            List<Bank> banks = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Bank bank = new()
                {
                    BankId = reader[0].ToString(),
                    BankName = reader[1].ToString()
                };
                banks.Add(bank);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return banks;
        }

        public async Task<Bank?> GetBankById(string id)
        {
            await _connection.OpenAsync();
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BankId, BankName FROM Banks WHERE BankId = @id AND IsActive = 1";
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Bank bank = new()
                {
                    BankId = reader[0].ToString(),
                    BankName = reader[1].ToString()
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return bank;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }

        public async Task<Bank?> GetBankByName(string bankName)
        {
            await _connection.OpenAsync();
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BankId, BankName FROM Banks WHERE BankName = @bankName AND IsActive = 1";
            command.Parameters.AddWithValue("@bankName", bankName);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                Bank bank = new()
                {
                    BankId = reader[0].ToString(),
                    BankName = reader[1].ToString()
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return bank;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }

        public async Task<bool> IsBankExist(string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BankId FROM Banks WHERE BankId = @BankId AND IsActive = 1";
            command.Parameters.AddWithValue("@BankId", bankId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isBankExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isBankExist;
        }

        public async Task<bool> AddBank(Bank bank)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Banks (BankName, BankId, IsActive) VALUES (@bankName, @bankId, @isActive)";
            command.Parameters.AddWithValue("@bankName", bank.BankName);
            command.Parameters.AddWithValue("@bankId", bank.BankId);
            command.Parameters.AddWithValue("@isActive", bank.IsActive);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBank(Bank bank)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("UPDATE Banks SET ");

            if (bank.BankName is not null)
            {
                queryBuilder.Append("BankName = @bankName, ");
                command.Parameters.AddWithValue("@bankName", bank.BankName);
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE BankId = @bankId and IsActive = 1");
            command.Parameters.AddWithValue("@bankId", bank.BankId);

            command.CommandText = queryBuilder.ToString();

            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }


        public async Task<bool> DeleteBank(string id)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE Banks SET IsActive=0 WHERE BankId=@id AND IsActive = 1";
            command.Parameters.AddWithValue("@id", id);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies(string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT CurrencyCode,ExchangeRate FROM Currencies WHERE IsActive = 1 and BankId = @bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            List<Currency> Currency = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Currency currency = new()
                {
                    CurrencyCode = reader[0].ToString(),
                    ExchangeRate = (decimal)reader[1]
                };
                Currency.Add(currency);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return Currency;
        }
    }
}
