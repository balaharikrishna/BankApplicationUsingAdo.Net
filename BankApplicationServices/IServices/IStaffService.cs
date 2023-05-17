using BankApplicationModels;
using BankApplicationModels.Enums;

namespace BankApplicationServices.IServices
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAllStaffAsync(string branchId);

        Task<Staff> GetStaffByIdAsync(string branchId, string staffAccountId);

        Task<Staff> GetStaffByNameAsync(string branchId, string staffName);
        /// <summary>
        /// Checks for staff Members account's with the given BankId and BranchId exists or not.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch to check.</param>
        /// <returns>A message indicating status of staff Account's Existence.</returns>
        Task<Message> IsStaffExistAsync(string branchId);

        /// <summary>
        /// Authenticates a staff account with the given BankId, BranchId, StaffAccountId, and StaffAccountPassword.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch to authenticate the staff account with.</param>
        /// <param name="staffAccountId">The StaffAccountId of the staff account to authenticate.</param>
        /// <param name="staffAccountPassword">The StaffAccountPassword of the staff account to authenticate.</param>
        /// <returns>A message indicating status of Staff Authentication.</returns>
        Task<Message> AuthenticateStaffAccountAsync(string branchId, string staffAccountId, string staffAccountPassword);

        /// <summary>
        /// Deletes a staff account with the given BankId, BranchId, and StaffAccountId.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch to delete the staff account from.</param>
        /// <param name="staffAccountId">The StaffAccountId of the staff account to delete.</param>
        /// <returns>A message indicating status of Staff Account Deletion.</returns>
        Task<Message> DeleteStaffAccountAsync(string branchId, string staffAccountId);

        /// <summary>
        /// Opens a new staff account with the given BankId, BranchId, StaffName, StaffPassword, and StaffRole.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch to open the staff account with.</param>
        /// <param name="staffName">The StaffName of the staff account to open.</param>
        /// <param name="staffPassword">The StaffPassword of the staff account to open.</param>
        /// <param name="staffRole">The StaffRole of the staff account to open.</param>
        /// <returns>A message indicating status of Staff Account Opening.</returns>
        Task<Message> OpenStaffAccountAsync(string branchId, string staffName, string staffPassword);

        /// <summary>
        /// Updates an existing staff account with the given BankId, BranchId, StaffAccountId, StaffName, StaffPassword, and StaffRole.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch to update the staff account with.</param>
        /// <param name="staffAccountId">The StaffAccountId of the staff account to update.</param>
        /// <param name="staffName">The StaffName of the staff account to update.</param>
        /// <param name="staffPassword">The StaffPassword of the staff account to update.</param>
        /// <param name="staffRole">The StaffRole of the staff account to update.</param>
        /// <returns>A message indicating status of staff Account Updation.</returns>
        Task<Message> UpdateStaffAccountAsync(string branchId, string staffAccountId, string staffName, string staffPassword, Roles staffRole);

        /// <summary>
        /// Checks if a staff account with the given BankId, BranchId, and StaffAccountId exists or not.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch to check.</param>
        /// <param name="staffAccountId">The StaffAccountId of the staff account to check.</param>
        /// <returns>A message indicating status of Staff Account Existence.</returns>
        Task<Message> IsAccountExistAsync(string branchId, string staffAccountId);

        /// <summary>
        /// Retrieves the details of a staff member from a specific bank and branch using their staff account ID.
        /// </summary>
        /// <param name="branchId">The ID of the branch where the staff member is located.</param>
        /// <param name="staffAccountId">The unique identifier for the staff member's account.</param>
        /// <returns>A string containing the staff member's details, including their name, position, and contact information.</returns>
    }
}