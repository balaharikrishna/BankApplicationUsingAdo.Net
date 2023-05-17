using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationRepository.Repository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class HeadManagerService : IHeadManagerService
    {
        private readonly IBankService _bankService;
        private readonly IEncryptionService _encryptionService;
        private readonly IHeadManagerRepository _headManagerRepository;

        public HeadManagerService(IHeadManagerRepository headManagerRepository, IBankService bankService, IEncryptionService encryptionService)
        {
            _bankService = bankService;
            _encryptionService = encryptionService;
            _headManagerRepository = headManagerRepository;
        }

        public async Task<IEnumerable<HeadManager>> GetAllHeadManagersAsync(string branchId)
        {
            return await _headManagerRepository.GetAllHeadManagers(branchId);
        }
     
        public async Task<HeadManager> GetHeadManagerByIdAsync(string bankId, string headManagerAccountId)
        {
            HeadManager headManager = await _headManagerRepository.GetHeadManagerById(headManagerAccountId, bankId);
            return headManager;
        }

        public async Task<HeadManager> GetHeadManagerByNameAsync(string bankId, string headManagerName)
        {
            HeadManager headManager = await _headManagerRepository.GetHeadManagerByName(headManagerName, bankId);
            return headManager;
        }
        public async Task<Message> IsHeadManagersExistAsync(string bankId)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
                IEnumerable<HeadManager> headManagers = await _headManagerRepository.GetAllHeadManagers(bankId);

                if (headManagers.Any())
                {
                    message.Result = true;
                    message.ResultMessage = $"Head Managers Exist in The Bank:{bankId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Head Managers Available In The Bank:{bankId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> IsHeadManagerExistAsync(string bankId, string headManagerAccountId)
        {
            Message message;
            message = await IsHeadManagersExistAsync(bankId);
            if (message.Result)
            {
                bool isHeadManagerExist = await _headManagerRepository.IsHeadManagerExist(headManagerAccountId, bankId);
                if (isHeadManagerExist)
                {
                    message.Result = true;
                    message.ResultMessage = "Head Manager Validation Successful.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Head Manager Validation Failed.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No Head Managers Available In The Bank:{bankId}";
            }
            return message;
        }

        public async Task<Message> AuthenticateHeadManagerAsync(string bankId, string headManagerAccountId, string headManagerPassword)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
                IEnumerable<HeadManager> headManagers = await _headManagerRepository.GetAllHeadManagers(bankId);
                if (headManagers.Any())
                {
                    byte[] salt = new byte[32];
                    HeadManager headManager = await _headManagerRepository.GetHeadManagerById(headManagerAccountId, bankId);
                    if (headManager is not null)
                    {
                        salt = headManager.Salt;
                    }

                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(headManagerPassword, salt);
                    bool isValidPassword = Convert.ToBase64String(headManager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                    if (isValidPassword)
                    {
                        message.Result = true;
                        message.ResultMessage = "Head Manager Validation Successful.";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Head Manager Validation Failed.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Head Managers Available In The Bank:{bankId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> OpenHeadManagerAccountAsync(string bankId, string headManagerName, string headManagerPassword)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
                HeadManager headManager = await _headManagerRepository.GetHeadManagerByName(headManagerName, bankId);
                
                if (headManager is null)
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string UserFirstThreeCharecters = headManagerName.Substring(0, 3);
                    string bankHeadManagerAccountId = string.Concat(UserFirstThreeCharecters, date);

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(headManagerPassword, salt);

                    HeadManager headManagerObject = new()
                    {
                        Name = headManagerName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = bankHeadManagerAccountId,
                        IsActive = true,
                        Role = Roles.HeadManager
                    };

                    bool isHeadManagerAdded = await _headManagerRepository.AddHeadManagerAccount(headManagerObject, bankId);
                    if (isHeadManagerAdded)
                    {
                        message.Result = true;
                        message.ResultMessage = $"Account Created for {headManagerName} with Account Id:{bankHeadManagerAccountId}";
                        message.Data = bankHeadManagerAccountId;
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Failed to Create HeadManager Account for {headManagerName}";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Head Manager: {headManagerName} Already Existed";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> UpdateHeadManagerAccountAsync(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword)
        {

            Message message;
            message = await IsHeadManagerExistAsync(bankId, headManagerAccountId);
            if (message.Result)
            {
                HeadManager headManager = await _headManagerRepository.GetHeadManagerById(headManagerAccountId, bankId);
                byte[] salt = null;
                byte[] hashedPassword = null;
                bool canContinue = true;
                if (headManagerPassword is not null && headManager is not null)
                {
                    salt = headManager!.Salt;
                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(headManagerPassword, salt);
                    if (Convert.ToBase64String(headManager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
                    {
                        message.Result = false;
                        message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        canContinue = false;
                    }
                    salt = _encryptionService.GenerateSalt();
                    hashedPassword = _encryptionService.HashPassword(headManagerPassword, salt);
                }

                if (canContinue && headManager is not null)
                {
                    HeadManager headManagerObject = new()
                    {
                        AccountId = headManagerAccountId,
                        Name = headManagerName,
                        HashedPassword = hashedPassword,
                        Salt = salt,
                        IsActive = true
                    };
                    bool isDetailsUpdated = await _headManagerRepository.UpdateHeadManagerAccount(headManagerObject, bankId);
                    if (isDetailsUpdated)
                    {
                        message.Result = true;
                        message.ResultMessage = "Updated Details Sucessfully";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Failed to Update Details";
                    }
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Head Manager Validation Failed.";
            }
            return message;
        }
        public async Task<Message> DeleteHeadManagerAccountAsync(string bankId, string headManagerAccountId)
        {
            Message message;
            message = await IsHeadManagerExistAsync(bankId, headManagerAccountId);
            if (message.Result)
            {
                bool isDeleted = await _headManagerRepository.DeleteHeadManagerAccount(headManagerAccountId, bankId);
                if (isDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{headManagerAccountId} Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete Head Manager Account Id:{headManagerAccountId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Head Manager with Account Id:{headManagerAccountId} doesn't Exist.";
            }
            return message;
        }

        public async Task<HeadManager> GetHeadManagerDetailsAsync(string bankId, string headManagerAccountId)
        {
            return await _headManagerRepository.GetHeadManagerById(headManagerAccountId, bankId);
        }
    }
}
