using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IBankService
    {
        /// <summary>
        /// Gets a banks Enumerable
        /// </summary>
        /// <returns>Returns an Enumerable of banks.</returns>
        Task<IEnumerable<Bank>> GetAllBanksAsync();

        Task<Bank?> GetBankByIdAsync(string id);

        Task<Bank?> GetBankByNameAsync(string name);

        /// <summary>
        /// Creates a new bank.
        /// </summary>
        /// <param name="bankName">Bank name for the new Bank</param>
        /// <returns>Message about the Status of Bank Creation.</returns>
        Task<Message> CreateBankAsync(string bankName);

        /// <summary>
        /// Deletes a Existing bank.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <returns>Message about the Status of Bank Deletion.</returns>
        Task<Message> DeleteBankAsync(string bankId);

        /// <summary>
        /// Retrieves the exchange rates for a specified bank.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank for which exchange rates are being retrieved.</param>
        /// <returns>A Message containing information about the status of the operation and the exchange rates retrieved.</returns>
        Task<IEnumerable<Currency>> GetExchangeRatesAsync(string bankId);

        /// <summary>
        /// Updates the name of a bank.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank being updated.</param>
        /// <param name="bankName">The new name of the bank.</param>
        /// <returns>A Message object containing information about the success or failure of the operation.</returns>
        Task<Message> UpdateBankAsync(string bankId, string bankName);

        /// <summary>
        /// Authenticates a bank's bankId to ensure that it is valid and authorized to perform certain actions within the system.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank being authenticated.</param>
        /// <returns>A Message containing information about the success or failure of the operation.</returns>
        Task<Message> AuthenticateBankIdAsync(string bankId);
    }
}