using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;

namespace BankApplicationRepository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;
        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<AuthenticateUser> GetUserAuthenticationDetails(string accountId,string name)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId, Name, Role, Salt, HashedPassword FROM(SELECT AccountId, Name, Salt, HashedPassword, Role, IsActive " +
            "FROM ReserveBankManagers UNION ALL SELECT AccountId, Name, Salt, HashedPassword, Role, IsActive " +
            "FROM HeadManagers UNION ALL SELECT AccountId, Name, Salt, HashedPassword, Role, IsActive " +
            "FROM Managers UNION ALL SELECT AccountId, Name, Salt, HashedPassword, Role, IsActive " +
            "FROM Staffs UNION ALL SELECT AccountId, Name, Salt, HashedPassword, Role, IsActive " +
            "FROM Customers) AS AllData WHERE IsActive = 1 AND AccountId = @accountId AND Name = @name ";
            command.Parameters.AddWithValue("@accountId", accountId);
            command.Parameters.AddWithValue("@name", name);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                AuthenticateUser user = new()
                {
                    AccountId = reader[0].ToString(),
                    Name = reader[1].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[2]),
                    Salt = (byte[])reader[3],
                    HashedPassword = (byte[])reader[4]
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return user;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
    }
}
