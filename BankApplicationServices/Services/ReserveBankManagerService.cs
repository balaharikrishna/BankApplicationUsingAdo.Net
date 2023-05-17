using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ReserveBankManagerService : IReserveBankManagerService
    {
        private readonly IReserveBankManagerRepository _reserveBankManagerRepository;
        private readonly IEncryptionService _encryptionService;
        public ReserveBankManagerService(IReserveBankManagerRepository reserveBankManagerRepository, IEncryptionService encryptionService)
        {
            _reserveBankManagerRepository = reserveBankManagerRepository;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<ReserveBankManager>> GetAllReserveBankManagersAsync()
        {
            return await _reserveBankManagerRepository.GetAllReserveBankManagers();
        }

        public async Task<ReserveBankManager> GetReserveBankManagerByIdAsync(string reserveBankManagerAccountId)
        {
            ReserveBankManager reserveBankManager = await _reserveBankManagerRepository.GetReserveBankManagerById(reserveBankManagerAccountId);
            return reserveBankManager;
        }
        public async Task<ReserveBankManager> GetReserveBankManagerByNameAsync(string reserveBankManagerName)
        {
            ReserveBankManager reserveBankManager = await _reserveBankManagerRepository.GetReserveBankManagerByName(reserveBankManagerName);
            return reserveBankManager;
        }

        public async Task<Message> AuthenticateManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerPassword)
        {
            Message message = new();
            IEnumerable<ReserveBankManager> reserveBankManagers = await _reserveBankManagerRepository.GetAllReserveBankManagers();
            if (reserveBankManagers.Any())
            {
                byte[] salt = new byte[32];
                ReserveBankManager reserveBankManager = await _reserveBankManagerRepository.GetReserveBankManagerById(ReserveBankManagerAccountId);
                if (reserveBankManager is not null)
                {
                    salt = reserveBankManager.Salt;
                }

                byte[] hashedPasswordToCheck = _encryptionService.HashPassword(ReserveBankManagerPassword, salt);
                bool isValidPassword = Convert.ToBase64String(reserveBankManager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                if (isValidPassword)
                {
                    message.Result = true;
                    message.ResultMessage = "ReserveBankManager Validation Successful.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "ReserveBankManager Validation Failed.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No ReserveBankManagera Available.";
            }
            return message;
        }
        public async Task<Message> IsReserveBankManagersExistAsync()
        {
            Message message = new();
            IEnumerable<ReserveBankManager> reserveBankManagers = await _reserveBankManagerRepository.GetAllReserveBankManagers();

            if (reserveBankManagers.Any())
            {
                message.Result = true;
                message.ResultMessage = $"reserveBankManagers Exist.";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No ReserveBankManagers Available";
            }
            return message;
        }
        public async Task<Message> OpenReserveBankManagerAccountAsync(string reserveBankManagerName, string reserveBankManagerPassword)
        {
            Message message = new();
            ReserveBankManager reserveBankManager = await _reserveBankManagerRepository.GetReserveBankManagerByName(reserveBankManagerName);


            if (reserveBankManager is null)
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string UserFirstThreeCharecters = reserveBankManagerName.Substring(0, 3);
                string reserveBankManagerAccountId = string.Concat(UserFirstThreeCharecters, date);

                byte[] salt = _encryptionService.GenerateSalt();
                byte[] hashedPassword = _encryptionService.HashPassword(reserveBankManagerPassword, salt);

                ReserveBankManager reserveBankManagerObject = new()
                {
                    Name = reserveBankManagerName,
                    Salt = salt,
                    HashedPassword = hashedPassword,
                    AccountId = reserveBankManagerAccountId,
                    IsActive = true,
                    Role = Roles.ReserveBankManager
                };

                bool isReserveBankManagerAdded = await _reserveBankManagerRepository.AddReserveBankManager(reserveBankManagerObject);
                if (isReserveBankManagerAdded)
                {
                    message.Result = true;
                    message.ResultMessage = $"Account Created for {reserveBankManagerName} with Account Id:{reserveBankManagerAccountId}";
                    message.Data = reserveBankManagerAccountId;
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Create Account for {reserveBankManagerName}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"reserveBankManager: {reserveBankManagerName} Already Existed";
            }
            return message;
        }
        public async Task<Message> UpdateReserveBankManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerName, string ReserveBankManagerPassword)
        {
            Message message;
            message = await IsAccountExistAsync(ReserveBankManagerAccountId);
            if (message.Result)
            {
                ReserveBankManager reserveBankManager = await _reserveBankManagerRepository.GetReserveBankManagerById(ReserveBankManagerAccountId);
                byte[] salt = null;
                byte[] hashedPassword = null;
                bool canContinue = true;
                if (ReserveBankManagerPassword is not null && reserveBankManager is not null)
                {
                    salt = reserveBankManager!.Salt;
                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(ReserveBankManagerPassword, salt);
                    if (Convert.ToBase64String(reserveBankManager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
                    {
                        message.Result = false;
                        message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        canContinue = false;
                    }
                    salt = _encryptionService.GenerateSalt();
                    hashedPassword = _encryptionService.HashPassword(ReserveBankManagerPassword, salt);
                }

                if (canContinue && reserveBankManager is not null)
                {
                    ReserveBankManager reserveBankManagerObject = new()
                    {
                        AccountId = ReserveBankManagerAccountId,
                        Name = ReserveBankManagerName,
                        HashedPassword = hashedPassword,
                        Salt = salt,
                        IsActive = true
                    };
                    bool isDetailsUpdated = await _reserveBankManagerRepository.UpdateReserveBankManager(reserveBankManagerObject);
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
                message.ResultMessage = "ReserveBankManager Validation Failed.";
            }
            return message;
        }
        public async Task<Message> DeleteReserveBankManagerAccountAsync(string ReserveBankManagerAccountId)
        {
            Message message;
            message = await IsAccountExistAsync(ReserveBankManagerAccountId);
            if (message.Result)
            {
                bool isDeleted = await _reserveBankManagerRepository.DeleteReserveBankManager(ReserveBankManagerAccountId);
                if (isDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{ReserveBankManagerAccountId} Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete ReserveBankManager Account Id:{ReserveBankManagerAccountId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"ReserveBankManager with Account Id:{ReserveBankManagerAccountId} doesn't Exist.";
            }
            return message;
        }
        public async Task<Message> IsAccountExistAsync(string ReserveBankManagerAccountId)
        {
            Message message;
            message = await IsReserveBankManagersExistAsync();
            if (message.Result)
            {
                bool isReserveBankManagerExist = await _reserveBankManagerRepository.IsReserveBankManagerExist(ReserveBankManagerAccountId);
                if (isReserveBankManagerExist)
                {
                    message.Result = true;
                    message.ResultMessage = "ReserveBankManager Exist.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "ReserveBankManager Doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No Managers Available";
            }
            return message;
        }
        public async Task<ReserveBankManager> GetReserveBankManagerDetailsAsync(string ReserveBankManagerAccountId)
        {
            return await _reserveBankManagerRepository.GetReserveBankManagerById(ReserveBankManagerAccountId);
        }
    }
}
