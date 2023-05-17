using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SqlConnection _connection;
        public CustomerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Customer?>> GetAllCustomers(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,AccountType,Name,Address,EmailId,PhoneNumber,DateOfBirth,Gender,PassbookIssueDate,Balance,Role" +
                " FROM Customers WHERE IsActive = 1 AND BranchId=@branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            List<Customer> customers = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Customer customer = new()
                {
                    AccountId = reader[0].ToString(),
                    AccountType = (AccountType)reader[1],
                    Name = reader[2].ToString(),
                    Address = reader[3].ToString(),
                    EmailId = reader[4].ToString(),
                    PhoneNumber = reader[5].ToString(),
                    DateOfBirth = reader[6].ToString(),
                    Gender = (Gender)reader[7],
                    PassbookIssueDate = reader[8].ToString(),
                    Balance = (decimal)reader[9],
                    Role = (Roles)Convert.ToUInt16(reader[10])
                };
                customers.Add(customer);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return customers;
        }
        public async Task<bool> AddCustomerAccount(Customer customer, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Customers (AccountId,Name,Salt,HashedPassword,Balance,Gender," +
                "AccountType,Address,DateOfBirth,EmailId,PhoneNumber,PassbookIssueDate,IsActive,Role,BranchId)" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@balance,@gender,@accountType,@address,@dateOfBirth," +
                "@emailId,@phoneNumber,@passbookIssueDate,@isActive,@role,@branchId)";
            command.Parameters.AddWithValue("@accountId", customer.AccountId);
            command.Parameters.AddWithValue("@name", customer.Name);
            command.Parameters.AddWithValue("@salt", customer.Salt);
            command.Parameters.AddWithValue("@hasedPassword", customer.HashedPassword);
            command.Parameters.AddWithValue("@balance", customer.Balance);
            command.Parameters.AddWithValue("@gender", customer.Gender);
            command.Parameters.AddWithValue("@accountType", customer.AccountType);
            command.Parameters.AddWithValue("@address", customer.Address);
            command.Parameters.AddWithValue("@dateOfBirth", customer.DateOfBirth);
            command.Parameters.AddWithValue("@emailId", customer.EmailId);
            command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            command.Parameters.AddWithValue("@passbookIssueDate", customer.PassbookIssueDate);
            command.Parameters.AddWithValue("@isActive", customer.IsActive);
            command.Parameters.AddWithValue("@role", customer.Role);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected>0;
        }

        public async Task<bool> UpdateCustomerAccount(Customer customer, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("UPDATE Customers SET ");

            if (customer.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", customer.Name);
            }

            if (customer.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", customer.Salt);

                if (customer.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", customer.HashedPassword);
                }
            }

            if (customer.Balance > 0)
            {
                queryBuilder.Append("Balance = @balance, ");
                command.Parameters.AddWithValue("@balance", customer.Balance);
            }

            if (Enum.IsDefined(typeof(Gender), customer.Gender))
            {
                queryBuilder.Append("Gender = @gender, ");
                command.Parameters.AddWithValue("@gender", customer.Gender);
            }

            if (Enum.IsDefined(typeof(AccountType), customer.AccountType))
            {
                queryBuilder.Append("AccountType = @accountType, ");
                command.Parameters.AddWithValue("@accountType", customer.AccountType);
            }

            if (customer.Address is not null)
            {
                queryBuilder.Append("Address = @address, ");
                command.Parameters.AddWithValue("@address", customer.Address);
            }

            if (customer.DateOfBirth is not null)
            {
                queryBuilder.Append("DateOfBirth = @dateOfBirth, ");
                command.Parameters.AddWithValue("@dateOfBirth", customer.DateOfBirth);
            }

            if (customer.EmailId is not null)
            {
                queryBuilder.Append("EmailId = @emailId, ");
                command.Parameters.AddWithValue("@emailId", customer.EmailId);
            }

            if (customer.PhoneNumber is not null)
            {
                queryBuilder.Append("PhoneNumber = @phoneNumber, ");
                command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            }

            if (customer.PassbookIssueDate is not null)
            {
                queryBuilder.Append("PassbookIssueDate = @passbookIssueDate, ");
                command.Parameters.AddWithValue("@passbookIssueDate", customer.PassbookIssueDate);
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE AccountId = @accountId AND BranchId = @branchId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", customer.AccountId);
            command.Parameters.AddWithValue("@branchId", branchId);

            command.CommandText = queryBuilder.ToString();

            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCustomerAccount(string customerAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE Customers SET IsActive = 0 WHERE AccountId=@customerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerAccountId", customerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsCustomerExist(string customerAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM Customers WHERE AccountId=@customerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerAccountId", customerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isCustomerExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isCustomerExist;
        }
        public async Task<Customer?> GetCustomerById(string customerAccountId, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,AccountType,Name,Address,EmailId,PhoneNumber,DateOfBirth,Gender,PassbookIssueDate,Balance,Salt," +
                "HashedPassword,Role FROM Customers WHERE AccountId=@customerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerAccountId", customerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Customer customer = new()
                {
                    AccountId = reader[0].ToString(),
                    AccountType = (AccountType)reader[1],
                    Name = reader[2].ToString(),
                    Address = reader[3].ToString(),
                    EmailId = reader[4].ToString(),
                    PhoneNumber = reader[5].ToString(),
                    DateOfBirth = reader[6].ToString(),
                    Gender = (Gender)reader[7],
                    PassbookIssueDate = reader[8].ToString(),
                    Balance = (decimal)reader[9],
                    Salt = (byte[])reader[10],
                    HashedPassword = (byte[])reader[11],
                    Role = (Roles)Convert.ToUInt16(reader[12])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return customer;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<Customer?> GetCustomerByName(string customerName, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId,AccountType,Name,Address,EmailId,PhoneNumber,DateOfBirth,Gender,PassbookIssueDate,Balance,Role " +
                "FROM Customers WHERE Name=@customerName and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerName", customerName);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Customer customer = new()
                {
                    AccountId = reader[0].ToString(),
                    AccountType = (AccountType)reader[1],
                    Name = reader[2].ToString(),
                    Address = reader[3].ToString(),
                    EmailId = reader[4].ToString(),
                    PhoneNumber = reader[5].ToString(),
                    DateOfBirth = reader[6].ToString(),
                    Gender = (Gender)reader[7],
                    PassbookIssueDate = reader[8].ToString(),
                    Balance = (decimal)reader[9],
                    Role = (Roles)Convert.ToUInt16(reader[10])
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return customer;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
    }
}
