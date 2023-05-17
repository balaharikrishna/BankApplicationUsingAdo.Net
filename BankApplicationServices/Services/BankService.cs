using BankApplicationModels;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class BankService : IBankService
    {

        private readonly IBankRepository _bankRepository;
        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<IEnumerable<Bank>> GetAllBanksAsync()
        {
            return await _bankRepository.GetAllBanks();
        }
        public async Task<Bank?> GetBankByIdAsync(string id)
        {
            return await _bankRepository.GetBankById(id);
        }
        public async Task<Bank?> GetBankByNameAsync(string name)
        {
            return await _bankRepository.GetBankByName(name);
        }

        public async Task<Message> AuthenticateBankIdAsync(string bankId)
        {
            Message message = new();
            bool isBankExist = await _bankRepository.IsBankExist(bankId);
            if (isBankExist)
            {
                message.Result = true;
                message.ResultMessage = $"Bank Id:'{bankId}' Exist.";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Bank Id:'{bankId}' Doesn't Exist.";
            }

            return message;
        }
        public async Task<Message> CreateBankAsync(string bankName)
        {
            Message message = new();

            Bank? bankDetails = await _bankRepository.GetBankByName(bankName);
            if (bankDetails != null)
            {
                message.Result = false;
                message.ResultMessage = $"BankName:{bankName} is Already Registered.";
            }
            else
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                string bankFirstThreeCharecters = bankName.Substring(0, 3).ToUpper();
                string bankId = bankFirstThreeCharecters + date + "M";

                Bank bank = new()
                {
                    BankName = bankName,
                    BankId = bankId,
                    IsActive = true
                };

                bool isBankAdded = await _bankRepository.AddBank(bank);
                if (isBankAdded)
                {
                    message.Result = true;
                    message.ResultMessage = $"Bank {bankName} is Created with {bankId}";
                    message.Data = bankId;
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Failed to Create a Bank";
                }
            }
            return message;
        }


        public async Task<Message> UpdateBankAsync(string bankId, string bankName)
        {
            Message message = new();

            Message messageResult = await AuthenticateBankIdAsync(bankId);
            if (messageResult.Result)
            {
                Bank bank = new()
                {
                    BankName = bankName,
                    BankId = bankId,
                    IsActive = true
                };
                bool isBankUpdated = await _bankRepository.UpdateBank(bank);
                if (isBankUpdated)
                {
                    message.Result = true;
                    message.ResultMessage = $"bankId :{bankId} is Updated with BankName : {bankName} Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Failed to Update Branch.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Bank Authentication Failed";
            }
            return message;
        }

        public async Task<Message> DeleteBankAsync(string bankId)
        {
            Message message = new();
            Message messageResult = await AuthenticateBankIdAsync(bankId);
            if (messageResult.Result)
            {
                bool isDeleted = await _bankRepository.DeleteBank(bankId);
                if (isDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Bank Id :{bankId} Succesfully Deleted.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete Bank Id :{bankId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Bank Authentication Failed";
            }
            return message;
        }

        public Task<IEnumerable<Currency>> GetExchangeRatesAsync(string bankId)
        {
            return _bankRepository.GetAllCurrencies(bankId);
        }
    }
}
