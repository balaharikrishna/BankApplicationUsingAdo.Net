using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAllCurrency(string bankId);
        Task<bool> AddCurrency(Currency currency, string bankId);
        Task<bool> UpdateCurrency(Currency currency, string bankId);
        Task<bool> DeleteCurrency(string currencyCode, string bankId);
        Task<bool> IsCurrencyExist(string currencyCode, string bankId);
        Task<Currency?> GetCurrencyByCode(string currencyCode, string bankId);
    }
}
