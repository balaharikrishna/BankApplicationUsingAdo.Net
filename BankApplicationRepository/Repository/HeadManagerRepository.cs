using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class HeadManagerRepository : IHeadManagerRepository
    {
        private readonly SqlConnection _connection;
        public HeadManagerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<HeadManager?>> GetAllHeadManagers(string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM HeadManagers WHERE IsActive = 1 and BankId=@bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            List<HeadManager> headManagers = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                HeadManager headManager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2])
                };
                headManagers.Add(headManager);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return headManagers;
        }

        public async Task<bool> AddHeadManagerAccount(HeadManager headManager, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO HeadManagers (AccountId,Name,Salt,HashedPassword,IsActive,Role,BankId )" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive,@role,@bankId)";
            command.Parameters.AddWithValue("@accountId", headManager.AccountId);
            command.Parameters.AddWithValue("@name", headManager.Name);
            command.Parameters.AddWithValue("@salt", headManager.Salt);
            command.Parameters.AddWithValue("@hasedPassword", headManager.HashedPassword);
            command.Parameters.AddWithValue("@isActive", headManager.IsActive);
            command.Parameters.AddWithValue("@role", headManager.Role);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateHeadManagerAccount(HeadManager headManager, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("UPDATE HeadManagers SET ");

            if (headManager.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", headManager.Name);
            }

            if (headManager.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", headManager.Salt);

                if (headManager.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", headManager.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);
            queryBuilder.Append(" WHERE AccountId = @accountId AND BankId = @bankId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", headManager.AccountId);
            command.Parameters.AddWithValue("@bankId", bankId);
            command.CommandText = queryBuilder.ToString();
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHeadManagerAccount(string headManagerAccountId, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE HeadManagers SET IsActive = 0 WHERE AccountId=@headManagerAccountId and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@headManagerAccountId", headManagerAccountId);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsHeadManagerExist(string headManagerAccountId, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM HeadManagers WHERE AccountId=@headManagerAccountId and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@headManagerAccountId", headManagerAccountId);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isHeadManagerExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isHeadManagerExist;
        }
        public async Task<HeadManager?> GetHeadManagerById(string headManagerAccountId, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Salt,HashedPassword,Role FROM HeadManagers WHERE AccountId=@headManagerAccountId and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@headManagerAccountId", headManagerAccountId);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var headManager = new HeadManager
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Salt = (byte[])reader[2],
                    HashedPassword = (byte[])reader[3],
                    Role = (Roles)Convert.ToUInt16(reader[4])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return headManager;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<HeadManager?> GetHeadManagerByName(string headManagerName, string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM HeadManagers WHERE Name=@headManagerName and BankId=@bankId AND IsActive = 1";
            command.Parameters.AddWithValue("@headManagerName", headManagerName);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                HeadManager headManager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return headManager;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
    }
}
