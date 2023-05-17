using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetAllCurrenciesAsync(string bankId);
        /// <summary>
        /// Adds a new currency to the bank with the specified exchange rate.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the currency will be added.</param>
        /// <param name="currencyCode">The code of the currency to be added.</param>
        /// <param name="exchangeRate">The exchange rate of the currency to be added.</param>
        /// <returns>A message about status of the currency addition.</returns>
        Task<Message> AddCurrencyAsync(string bankId, string currencyCode, decimal exchangeRate);

        Task<Currency?> GetCurrencyByCode(string currencyCode, string bankId);
        /// <summary>
        /// Deletes an existing currency from the bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the currency will be deleted.</param>
        /// <param name="currencyCode">The code of the currency to be deleted.</param>
        /// <returns>A message about status of the currency deletion.</returns>
        Task<Message> DeleteCurrencyAsync(string bankId, string currencyCode);

        /// <summary>
        /// Updates the exchange rate of an existing currency in the bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the currency exchange rate will be updated.</param>
        /// <param name="currencyCode">The code of the currency whose exchange rate will be updated.</param>
        /// <param name="exchangeRate">The new exchange rate of the currency.</param>
        /// <returns>A message about status of the Currency Updation.</returns>
        Task<Message> UpdateCurrencyAsync(string bankId, string currencyCode, decimal exchangeRate);

        /// <summary>
        /// Validates whether a currency with the specified code exists in the bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the currency will be validated.</param>
        /// <param name="currencyCode">The code of the currency to be validated.</param>
        /// <returns>A message about status of the currency validation.</returns>
        Task<Message> ValidateCurrencyAsync(string bankId, string currencyCode);

        public readonly static string? defaultCurrencyCode;
        public readonly static short defaultCurrencyValue;
    }
}