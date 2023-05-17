using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class StaffRepository : IStaffRepository
    {
        private readonly SqlConnection _connection;
        public StaffRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Staff>> GetAllStaffs(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM Staffs WHERE IsActive = 1 AND BranchId=@branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            List<Staff> staffs = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Staff staff = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2])
                };
                staffs.Add(staff);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return staffs;
        }
        public async Task<bool> AddStaffAccount(Staff staff, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Staffs (AccountId,Name,Salt,HashedPassword,IsActive,Role,BranchId )" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive,@role,@branchId)";
            command.Parameters.AddWithValue("@accountId", staff.AccountId);
            command.Parameters.AddWithValue("@name", staff.Name);
            command.Parameters.AddWithValue("@salt", staff.Salt);
            command.Parameters.AddWithValue("@hasedPassword", staff.HashedPassword);
            command.Parameters.AddWithValue("@isActive", staff.IsActive);
            command.Parameters.AddWithValue("@role", staff.Role);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
       
        public async Task<bool> UpdateStaffAccount(Staff staff, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("UPDATE Staffs SET ");

            if (staff.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", staff.Name);
            }

            if (staff.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", staff.Salt);

                if (staff.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", staff.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);
            queryBuilder.Append(" WHERE AccountId = @accountId AND BranchId = @branchId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", staff.AccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            command.CommandText = queryBuilder.ToString();
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteStaffAccount(string staffAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE Staffs SET IsActive = 0 WHERE AccountId=@staffAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffAccountId", staffAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsStaffExist(string staffAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM Staffs WHERE AccountId=@staffAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffAccountId", staffAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isStaffExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isStaffExist;
        }
        public async Task<Staff?> GetStaffById(string staffAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Salt,HashedPassword,Role FROM Staffs WHERE AccountId=@staffAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffAccountId", staffAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Staff staff = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Salt = (byte[])reader[2],
                    HashedPassword = (byte[])reader[3],
                    Role = (Roles)Convert.ToUInt16(reader[4])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return staff;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<Staff?> GetStaffByName(string staffName, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,Name,Role FROM Staffs WHERE Name=@staffName and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffName", staffName);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Staff staff = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return staff;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
    }
}
