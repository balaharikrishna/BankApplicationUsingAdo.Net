using BankApplicationModels;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IBankService _bankService;
        public CurrencyService(ICurrencyRepository currencyRepository, IBankService bankService)
        {
            _currencyRepository = currencyRepository;
            _bankService = bankService;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync(string bankId)
        {
            return await _currencyRepository.GetAllCurrency(bankId);
        }
        public async Task<Message> ValidateCurrencyAsync(string bankId, string currencyCode)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
                bool isCurrencyExist = await _currencyRepository.IsCurrencyExist(currencyCode, bankId);

                if (isCurrencyExist)
                {
                    message.Result = true;
                    message.ResultMessage = $"Currency Code:'{currencyCode}' is Exist";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Currency Code:'{currencyCode}' doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public async Task<Currency?> GetCurrencyByCode(string currencyCode, string bankId)
        {
            return await _currencyRepository.GetCurrencyByCode(currencyCode, bankId);
        }
        public async Task<Message> AddCurrencyAsync(string bankId, string currencyCode, decimal exchangeRate)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
                Currency currency = new()
                {
                    ExchangeRate = exchangeRate,
                    CurrencyCode = currencyCode,
                    IsActive = true
                };
                bool isCurrencyAdded = await _currencyRepository.AddCurrency(currency, bankId);
                if (isCurrencyAdded)
                {
                    message.Result = true;
                    message.ResultMessage = $"Added Currency Code:{currencyCode} with Exchange Rate:{exchangeRate}";
                    message.Data = currencyCode;
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Failed to Add Currency";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> UpdateCurrencyAsync(string bankId, string currencyCode, decimal exchangeRate)
        {
            Message message;
            message = await ValidateCurrencyAsync(bankId, currencyCode);
            if (message.Result)
            {
                Currency currency = new()
                {
                    ExchangeRate = exchangeRate,
                    CurrencyCode = currencyCode,
                    IsActive = true
                };
                bool isCurrencyUpdated = await _currencyRepository.UpdateCurrency(currency, bankId);
                if (isCurrencyUpdated)
                {
                    message.Result = true;
                    message.ResultMessage = $"Currency Code :{currencyCode} updated with Exchange Rate :{exchangeRate}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Update Currency:{currencyCode}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Currency Code :{currencyCode} not Found";
            }
            return message;
        }

        public async Task<Message> DeleteCurrencyAsync(string bankId, string currencyCode)
        {
            Message message;
            message = await ValidateCurrencyAsync(bankId, currencyCode);
            if (message.Result)
            {
                bool isCurrencyDeleted = await _currencyRepository.DeleteCurrency(currencyCode, bankId);
                if (isCurrencyDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Currency Code :{currencyCode} Deleted Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete CurrencyCode:{currencyCode}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Currency Code :{currencyCode} not Found";
            }
            return message;
        }
    }
}
