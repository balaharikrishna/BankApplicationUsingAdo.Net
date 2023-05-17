using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationRepository.Repository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IBranchService _branchService;
        private readonly IEncryptionService _encryptionService;
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IEncryptionService encryptionService, IManagerRepository managerRepository,
            IBranchService branchService)
        {
            _encryptionService = encryptionService;
            _branchService = branchService;
            _managerRepository = managerRepository;
        }

        public async Task<IEnumerable<Manager>> GetAllManagersAsync(string branchId)
        {
            return await _managerRepository.GetAllManagers(branchId);
        }
    
        public async Task<Manager> GetManagerByIdAsync(string branchId, string managerAccountId)
        {
            Manager manager = await _managerRepository.GetManagerById(managerAccountId,branchId);
            return manager;
        }

        public async Task<Manager> GetManagerByNameAsync(string branchId, string managerName)
        {
            Manager manager = await _managerRepository.GetManagerByName(managerName, branchId);
            return manager;
        }
        public async Task<Message> IsManagersExistAsync(string branchId)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                IEnumerable<Manager> managers = await _managerRepository.GetAllManagers(branchId);

                if (managers.Any())
                {
                    message.Result = true;
                    message.ResultMessage = $"Managers Exist in The Branch:{branchId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Managers Available In The Branch:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> AuthenticateManagerAccountAsync(string branchId,
            string managerAccountId, string managerPassword)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                IEnumerable<Manager> managers = await _managerRepository.GetAllManagers(branchId);
                if (managers.Any())
                {
                    byte[] salt = new byte[32];
                    Manager manager = await _managerRepository.GetManagerById(managerAccountId, branchId);
                    if (manager is not null)
                    {
                        salt = manager.Salt;
                    }

                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(managerPassword, salt);
                    bool isValidPassword = Convert.ToBase64String(manager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                    if (isValidPassword)
                    {
                        message.Result = true;
                        message.ResultMessage = "Manager Validation Successful.";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Manager Validation Failed.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Managers Available In The Branch Id:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> OpenManagerAccountAsync(string branchId, string managerName, string managerPassword)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                Manager manager = await _managerRepository.GetManagerByName(managerName, branchId);
               
                if (manager is null)
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string UserFirstThreeCharecters = managerName.Substring(0, 3);
                    string managerAccountId = string.Concat(UserFirstThreeCharecters, date);

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(managerPassword, salt);

                    Manager managerObject = new()
                    {
                        Name = managerName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = managerAccountId,
                        IsActive = true,
                        Role = Roles.Manager
                    };

                    bool isManagerAdded = await _managerRepository.AddManagerAccount(managerObject, branchId);
                    if (isManagerAdded)
                    {
                        message.Result = true;
                        message.ResultMessage = $"Account Created for {managerName} with Account Id:{managerAccountId}";
                        message.Data = managerAccountId;
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Failed to Create Manager Account for {managerName}";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Manager: {managerName} Already Existed";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> IsAccountExistAsync(string branchId, string managerAccountId)
        {
            Message message;
            message = await IsManagersExistAsync(branchId);
            if (message.Result)
            {
                bool isManagerExist = await _managerRepository.IsManagerExist(managerAccountId, branchId);
                if (isManagerExist)
                {
                    message.Result = true;
                    message.ResultMessage = "Manager Exist.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Manager Doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No Managers Available In The Branch:{branchId}";
            }
            return message;
        }

        public async Task<Message> UpdateManagerAccountAsync(string branchId, string accountId, string managerName, string managerPassword)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, accountId);
            if (message.Result)
            {
                Manager manager = await _managerRepository.GetManagerById(accountId, branchId);
                byte[] salt = null;
                byte[] hashedPassword = null;
                bool canContinue = true;
                if (managerPassword is not null && manager is not null)
                {
                    salt = manager!.Salt;
                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(managerPassword, salt);
                    if (Convert.ToBase64String(manager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
                    {
                        message.Result = false;
                        message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        canContinue = false;
                    }
                    salt = _encryptionService.GenerateSalt();
                    hashedPassword = _encryptionService.HashPassword(managerPassword, salt);
                }

                if (canContinue && manager is not null)
                {
                    Manager managerObject = new()
                    {
                        AccountId = accountId,
                        Name = managerName,
                        HashedPassword = hashedPassword,
                        Salt = salt,
                        IsActive = true
                    };
                    bool isDetailsUpdated = await _managerRepository.UpdateManagerAccount(managerObject, branchId);
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
                message.ResultMessage = "Manager Validation Failed.";
            }
            return message;
        }

        public async Task<Message> DeleteManagerAccountAsync(string branchId, string accountId)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, accountId);
            if (message.Result )
            {
                bool isDeleted = await _managerRepository.DeleteManagerAccount(accountId, branchId);
                if (isDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{accountId} Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete Manager Account Id:{accountId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Manager with Account Id:{accountId} doesn't Exist.";
            }
            return message;
        }
    }
}
