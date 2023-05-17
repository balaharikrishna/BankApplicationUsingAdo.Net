using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IHeadManagerService
    {
        Task<IEnumerable<HeadManager>> GetAllHeadManagersAsync(string branchId);

        Task<HeadManager> GetHeadManagerByIdAsync(string bankId, string headManagerAccountId);

        Task<HeadManager> GetHeadManagerByNameAsync(string bankId, string headManagerName);
        
        /// <summary>
        /// Checks if any Head Managers exist for the given BankId.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <returns>Message indicating the status of Head Managers Existance.</returns>
        Task<Message> IsHeadManagersExistAsync(string bankId);

        /// <summary>
        /// Authenticates a Head Manager.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <param name="headManagerPassword">Password of the Head Manager</param>
        /// <returns>Message indicating the Status of Authentication.</returns>
        Task<Message> AuthenticateHeadManagerAsync(string bankId, string headManagerAccountId, string headManagerPassword);

        /// <summary>
        /// Opens a Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerName">Name of the Head Manager</param>
        /// <param name="headManagerPassword">Password of the Head Manager</param>
        /// <returns>Message indicating the status of Account Creation.</returns>
        Task<Message> OpenHeadManagerAccountAsync(string bankId, string headManagerName, string headManagerPassword);

        /// <summary>
        /// Deletes the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Message indicating the Status of Account Deletion.</returns>
        Task<Message> DeleteHeadManagerAccountAsync(string bankId, string headManagerAccountId);

        /// <summary>
        /// Updates the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <param name="headManagerName">New name for the Head Manager</param>
        /// <param name="headManagerPassword">New password for the Head Manager</param>
        /// <returns>Message indicating status of Account Updation.</returns>
        Task<Message> UpdateHeadManagerAccountAsync(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword);

        /// <summary>
        /// Checks for Head Manager account Existence.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Message indicating status of account Existence.</returns>
        Task<Message> IsHeadManagerExistAsync(string bankId, string headManagerAccountId);

        /// <summary>
        /// Provides details of the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Details of the Head Manager account.</returns>
        Task<HeadManager> GetHeadManagerDetailsAsync(string bankId, string headManagerAccountId);

    }
}