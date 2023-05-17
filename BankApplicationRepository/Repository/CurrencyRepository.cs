using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly SqlConnection _connection;
        public CurrencyRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrency(string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT CurrencyCode,ExchangeRate FROM Currencies WHERE IsActive = 1 AND BankId=@bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            List<Currency> currencies = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var currency = new Currency
                {
                    CurrencyCode = reader[0].ToString(),
                    ExchangeRate = (decimal)reader[1]
                };
                currencies.Add(currency);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return currencies;
        }
        public async Task<bool> AddCurrency(Currency currency, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Currencies (CurrencyCode,ExchangeRate,IsActive,BankId)" +
                " VALUES (@currencyCode, @exchangeRate, @isActive,@bankId)";
            command.Parameters.AddWithValue("@currencyCode", currency.CurrencyCode);
            command.Parameters.AddWithValue("@exchangeRate", currency.ExchangeRate);
            command.Parameters.AddWithValue("@isActive", currency.IsActive);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateCurrency(Currency currency, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder query = new("UPDATE Currencies SET ");

            if (currency.CurrencyCode is not null)
            {
                query.Append("ExchangeRate=@exchangeRate,");
                command.Parameters.AddWithValue("@exchangeRate", currency.ExchangeRate);
            }
            query.Remove(query.Length - 1, 1);
            query.Append(" WHERE BankId=@bankId AND IsActive = 1 AND CurrencyCode=@currencyCode");
            command.Parameters.AddWithValue("@bankId", bankId);
            command.Parameters.AddWithValue("@currencyCode", currency.CurrencyCode);
            command.CommandText = query.ToString();
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected>0;
        }

        public async Task<bool> DeleteCurrency(string currencyCode, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE Currencies SET IsActive = 0 WHERE CurrencyCode=@currencyCode and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@currencyCode", currencyCode);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> IsCurrencyExist(string currencyCode, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT CurrencyCode FROM Currencies WHERE BankId = @bankId and CurrencyCode=@currencyCode AND IsActive = 1";
            command.Parameters.AddWithValue("@currencyCode", currencyCode);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isCurrencyExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isCurrencyExist;
        }

        public async Task<Currency?> GetCurrencyByCode(string currencyCode, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT CurrencyCode,ExchangeRate FROM Currencies WHERE CurrencyCode = @currencyCode AND IsActive = 1 AND BankId=@bankId";
            command.Parameters.AddWithValue("@currencyCode", currencyCode);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Currency currency = new()
                {
                    CurrencyCode = reader[0].ToString(),
                    ExchangeRate = (decimal)reader[1]
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return currency;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
    }
}
