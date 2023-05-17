using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class ReserveBankManagerRepository : IReserveBankManagerRepository
    {
        private readonly SqlConnection _connection;
        public ReserveBankManagerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<ReserveBankManager>> GetAllReserveBankManagers()
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM ReserveBankManagers WHERE IsActive = 1";
            List<ReserveBankManager> reserveBankManagers = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ReserveBankManager reserveBankManager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2])
                };
                reserveBankManagers.Add(reserveBankManager);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return reserveBankManagers;
        }
        public async Task<bool> AddReserveBankManager(ReserveBankManager reserveBankManager)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO ReserveBankManagers (AccountId,Name,Salt,HashedPassword,IsActive,Role )" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive,@role)";
            command.Parameters.AddWithValue("@accountId", reserveBankManager.AccountId);
            command.Parameters.AddWithValue("@name", reserveBankManager.Name);
            command.Parameters.AddWithValue("@salt", reserveBankManager.Salt);
            command.Parameters.AddWithValue("@hasedPassword", reserveBankManager.HashedPassword);
            command.Parameters.AddWithValue("@isActive", reserveBankManager.IsActive);
            command.Parameters.AddWithValue("@role", reserveBankManager.Role);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
       
        public async Task<bool> UpdateReserveBankManager(ReserveBankManager reserveBankManager)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("UPDATE ReserveBankManagers SET ");

            if (reserveBankManager.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", reserveBankManager.Name);
            }

            if (reserveBankManager.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", reserveBankManager.Salt);

                if (reserveBankManager.HashedPassword != null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", reserveBankManager.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);
            queryBuilder.Append(" WHERE AccountId = @accountId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", reserveBankManager.AccountId);
            command.CommandText = queryBuilder.ToString();
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReserveBankManager(string reserveBankManagerAccountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE ReserveBankManagers SET IsActive = 0 WHERE AccountId=@reserveBankManagerAccountId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@reserveBankManagerAccountId", reserveBankManagerAccountId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsReserveBankManagerExist(string reserveBankManagerAccountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM ReserveBankManagers WHERE AccountId=@reserveBankManagerAccountId  AND IsActive = 1 ";
            command.Parameters.AddWithValue("@reserveBankManagerAccountId", reserveBankManagerAccountId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isReserveBankManagerExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isReserveBankManagerExist;
        }
        public async Task<ReserveBankManager?> GetReserveBankManagerById(string reserveBankManagerAccountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Salt,HashedPassword,Role FROM ReserveBankManagers WHERE AccountId=@reserveBankManagerAccountId AND IsActive = 1";
            command.Parameters.AddWithValue("@reserveBankManagerAccountId", reserveBankManagerAccountId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                ReserveBankManager reserveBankManager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Salt = (byte[])reader[2],
                    HashedPassword = (byte[])reader[3],
                    Role = (Roles)Convert.ToUInt16(reader[4]),
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return reserveBankManager;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<ReserveBankManager?> GetReserveBankManagerByName(string reserveBankManagerName)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM ReserveBankManagers WHERE Name=@reserveBankManagerName AND IsActive = 1";
            command.Parameters.AddWithValue("@reserveBankManagerName", reserveBankManagerName);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                ReserveBankManager reserveBankManager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role= (Roles)Convert.ToUInt16(reader[2])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return reserveBankManager;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
    }
}
