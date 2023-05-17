using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IManagerService
    {
        Task<IEnumerable<Manager>> GetAllManagersAsync(string branchId);

        /// <summary>
        /// Retrieves the details of a branch manager's Account.
        /// </summary>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="managerAccountId">The account ID of the branch manager.</param>
        /// <returns>A string containing the details of the branch manager's account.</returns>
        Task<Manager> GetManagerByIdAsync(string branchId, string managerAccountId);

        Task<Manager> GetManagerByNameAsync(string branchId, string managerName);
        /// <summary>
        /// Checks for managers Existence.
        /// </summary>
        /// <param name="branchId">The ID of the branch.</param>
        /// <returns>A message indicating Status of managers Existence.</returns>
        Task<Message> IsManagersExistAsync(string branchId);

        /// <summary>
        /// Authenticates the Manager Account.
        /// </summary>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="branchManagerAccountId">The account ID of the branch manager.</param>
        /// <param name="branchManagerPassword">The password of the branch manager.</param>
        /// <returns>A message indicating status of Authentication.</returns>
        Task<Message> AuthenticateManagerAccountAsync(string branchId, string branchManagerAccountId, string branchManagerPassword);

        /// <summary>
        /// Deletes Manager Account.
        /// </summary>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="accountId">The account ID of the branch manager to delete.</param>
        /// <returns>A message indicating status of Account Deletion.</returns>
        Task<Message> DeleteManagerAccountAsync(string branchId, string accountId);

        /// <summary>
        /// Opens a new Account for Manager.
        /// </summary>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="branchManagerName">The name of the branch manager.</param>
        /// <param name="branchManagerPassword">The password for the branch manager's account.</param>
        /// <returns>A message indicating status of Account Opening.</returns>
        Task<Message> OpenManagerAccountAsync(string branchId, string branchManagerName, string branchManagerPassword);

        /// <summary>
        /// Updates manager's Account.
        /// </summary>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="accountId">The account ID of the branch manager to update.</param>
        /// <param name="branchManagerName">The new name for the branch manager.</param>
        /// <param name="branchManagerPassword">The new password for the branch manager's account.</param>
        /// <returns>A message indicating status of Account Updation.</returns>
        Task<Message> UpdateManagerAccountAsync(string branchId, string accountId, string branchManagerName, string branchManagerPassword);

        /// <summary>
        /// Checks for Account Existence.
        /// </summary>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="managerAccountId">The account ID of the branch manager to check for.</param>
        /// <returns>A message indicating status of Account Existence.</returns>
        Task<Message> IsAccountExistAsync(string branchId, string managerAccountId);

    }
}