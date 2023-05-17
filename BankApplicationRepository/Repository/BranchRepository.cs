using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly SqlConnection _connection;
        public BranchRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Branch>> GetAllBranches(string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BranchId,BranchName,BranchAddress,BranchPhoneNumber FROM Branches WHERE IsActive = 1 and BankId=@bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            List<Branch> branches = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Branch branch = new()
                {
                    BranchId = reader[0].ToString(),
                    BranchName = reader[1].ToString(),
                    BranchAddress = reader[2].ToString(),
                    BranchPhoneNumber = reader[3].ToString()
                };
                branches.Add(branch);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return branches;
        }

        public async Task<Branch?> GetBranchById(string branchId)
        {
            await _connection.OpenAsync();
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BranchId,BranchName,BranchAddress,BranchPhoneNumber FROM Branches WHERE BranchId = @branchId AND IsActive = 1";
            command.Parameters.AddWithValue("@branchId", branchId);
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Branch branch = new()
                {
                    BranchId = reader[0].ToString(),
                    BranchName = reader[1].ToString(),
                    BranchAddress = reader[2].ToString(),
                    BranchPhoneNumber = reader[3].ToString()
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return branch;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }

        public async Task<bool> IsBranchExist(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BranchId FROM Branches WHERE BranchId = @branchId AND IsActive = 1";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isBranchExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isBranchExist;
        }

        public async Task<bool> AddBranch(Branch branch,string bankId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Branches (BranchName, BranchId, IsActive,BranchAddress,BranchPhoneNumber,BankId)" +
                " VALUES (@branchName, @branchId, @isActive,@branchAddress,@branchPhoneNumber,@bankId)";
            command.Parameters.AddWithValue("@branchName", branch.BranchName);
            command.Parameters.AddWithValue("@branchId", branch.BranchId);
            command.Parameters.AddWithValue("@isActive", branch.IsActive);
            command.Parameters.AddWithValue("@branchAddress", branch.BranchAddress);
            command.Parameters.AddWithValue("@branchPhoneNumber", branch.BranchPhoneNumber);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected>0;
        }

        public async Task<bool> UpdateBranch(Branch branch)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder query = new("UPDATE Branches SET ");

            if (!string.IsNullOrEmpty(branch.BranchName))
            {
                query.Append("BranchName=@branchName,");
                command.Parameters.AddWithValue("@branchName", branch.BranchName);
            }

            if (!string.IsNullOrEmpty(branch.BranchAddress))
            {
                query.Append("BranchAddress=@branchAddress,");
                command.Parameters.AddWithValue("@branchAddress", branch.BranchAddress);
            }

            if (!string.IsNullOrEmpty(branch.BranchPhoneNumber))
            {
                query.Append("BranchPhoneNumber=@branchPhoneNumber,");
                command.Parameters.AddWithValue("@branchPhoneNumber", branch.BranchPhoneNumber);
            }

            query.Remove(query.Length - 1, 1); 
            query.Append(" WHERE BranchId=@branchId and IsActive=1");
            command.Parameters.AddWithValue("@branchId", branch.BranchId);

            command.CommandText = query.ToString();
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBranch(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE Branches SET IsActive = 0 WHERE BranchId=@branchId AND IsActive = 1";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<Branch?> GetBranchByName(string branchName)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BranchId,BranchName,BranchAddress,BranchPhoneNumber FROM Branches WHERE BranchName = @branchName AND IsActive = 1";
            command.Parameters.AddWithValue("@branchName", branchName);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Branch branch = new()
                {
                    BranchId = reader[0].ToString(),
                    BranchName = reader[1].ToString(),
                    BranchAddress = reader[2].ToString(),
                    BranchPhoneNumber = reader[3].ToString()
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return branch;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }

        public async Task<IEnumerable<TransactionCharges>> GetAllTransactionCharges(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT RtgsOtherBank,RtgsSameBank,ImpsOtherBank,ImpsSameBank FROM TransactionCharges WHERE IsActive = 1 and BranchId = @branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            List<TransactionCharges> transactionChargesList = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var transactionCharges = new TransactionCharges
                {
                    RtgsOtherBank = (ushort)reader[0],
                    RtgsSameBank  = (ushort)reader[1],
                    ImpsOtherBank = (ushort)reader[2],
                    ImpsSameBank  = (ushort)reader[3]
                };
                transactionChargesList.Add(transactionCharges);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return transactionChargesList;
        }
    }
}
