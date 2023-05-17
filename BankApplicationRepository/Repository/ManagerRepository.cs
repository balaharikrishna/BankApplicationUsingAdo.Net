using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly SqlConnection _connection;
        public ManagerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Manager?>> GetAllManagers(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM Managers WHERE IsActive = 1 AND BranchId=@branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            List<Manager> managers = new(); 
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Manager manager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2])
                };
                managers.Add(manager);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return managers;
        }
        public async Task<bool> AddManagerAccount(Manager manager, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Managers (AccountId,Name,Salt,HashedPassword,IsActive,Role,BranchId )" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive,@role,@branchId)";
            command.Parameters.AddWithValue("@accountId", manager.AccountId);
            command.Parameters.AddWithValue("@name", manager.Name);
            command.Parameters.AddWithValue("@salt", manager.Salt);
            command.Parameters.AddWithValue("@hasedPassword", manager.HashedPassword);
            command.Parameters.AddWithValue("@isActive", manager.IsActive);
            command.Parameters.AddWithValue("@role", manager.Role);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateManagerAccount(Manager manager, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("UPDATE Managers SET ");

            if (manager.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", manager.Name);
            }

            if (manager.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", manager.Salt);

                if (manager.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", manager.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);
            queryBuilder.Append(" WHERE AccountId = @accountId AND BranchId = @branchId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", manager.AccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            command.CommandText = queryBuilder.ToString();
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteManagerAccount(string managerAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE Managers SET IsActive = 0 WHERE AccountId=@managerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerAccountId", managerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsManagerExist(string managerAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM Managers WHERE AccountId=@managerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerAccountId", managerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isManagerExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isManagerExist;
        }
        public async Task<Manager?> GetManagerById(string managerAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Salt,HashedPassword,Role FROM Managers WHERE AccountId=@managerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerAccountId", managerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Manager manager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Salt = (byte[])reader[2],
                    HashedPassword = (byte[])reader[3],
                    Role = (Roles)Convert.ToUInt16(reader[4])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return manager;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<Manager?> GetManagerByName(string managerName, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM Managers WHERE Name=@managerName and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerName", managerName);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Manager manager = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return manager;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }

    }
}
