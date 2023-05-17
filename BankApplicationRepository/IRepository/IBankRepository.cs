using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface IBankRepository
    {
        Task<bool> AddBank(Bank bank);
        Task<bool> DeleteBank(string id);
        Task<IEnumerable<Bank>> GetAllBanks();
        Task<Bank?> GetBankById(string id);
        Task<bool> UpdateBank(Bank bank);
        Task<bool> IsBankExist(string bankId);
        Task<Bank?> GetBankByName(string bankName);
        Task<IEnumerable<Currency>> GetAllCurrencies(string bankId);
    }
}