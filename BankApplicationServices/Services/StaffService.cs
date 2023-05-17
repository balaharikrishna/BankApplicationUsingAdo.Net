using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationRepository.Repository;
using BankApplicationServices.IServices;
using System.Data;

namespace BankApplicationServices.Services
{
    public class StaffService : IStaffService
    {
        private readonly IBranchService _branchService;
        private readonly IEncryptionService _encryptionService;
        private readonly IStaffRepository _staffRepository;
        public StaffService(IBranchService branchService, IEncryptionService encryptionService, IStaffRepository staffRepository)
        {
            _branchService = branchService;
            _encryptionService = encryptionService;
            _staffRepository = staffRepository;
        }

        public async Task<IEnumerable<Staff>> GetAllStaffAsync(string branchId)
        {
            return await _staffRepository.GetAllStaffs(branchId);
        }

        public async Task<Staff> GetStaffByIdAsync(string branchId, string staffAccountId)
        {
            Staff staff = await _staffRepository.GetStaffById(staffAccountId, branchId);
            return staff;
        }

        public async Task<Staff> GetStaffByNameAsync(string branchId, string staffName)
        {
            Staff staff = await _staffRepository.GetStaffByName(staffName, branchId);
            return staff;
        }
        public async Task<Message> IsStaffExistAsync(string branchId)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                IEnumerable<Staff> staffs = await _staffRepository.GetAllStaffs(branchId);

                if (staffs.Any())
                {
                    message.Result = true;
                    message.ResultMessage = $"Staff Exist in The Branch:{branchId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Staff Available In The Branch:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "branchId Authentication Failed";
            }

            return message;
        }
        public async Task<Message> AuthenticateStaffAccountAsync(string branchId,
           string staffAccountId, string staffAccountPassword)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                IEnumerable<Staff> staffs = await _staffRepository.GetAllStaffs(branchId);
                if (staffs.Any())
                {
                    byte[] salt = new byte[32];
                    Staff staff = await _staffRepository.GetStaffById(staffAccountId, branchId);
                    if (staff is not null)
                    {
                        salt = staff.Salt;
                    }

                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(staffAccountPassword, salt);
                    bool isValidPassword = Convert.ToBase64String(staff.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                    if (isValidPassword)
                    {
                        message.Result = true;
                        message.ResultMessage = "Staff Validation Successful.";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Staff Validation Failed.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Staff Available In The Branch Id:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> OpenStaffAccountAsync(string branchId, string staffName, string staffPassword)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                Staff staff = await _staffRepository.GetStaffByName(staffName, branchId);

                if (staff is null)
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string UserFirstThreeCharecters = staffName.Substring(0, 3);
                    string staffAccountId = string.Concat(UserFirstThreeCharecters, date);

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(staffPassword, salt);

                    Staff staffObject = new()
                    {
                        Name = staffName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = staffAccountId,
                        IsActive = true,
                        Role = Roles.Staff
                    };

                    bool isManagerAdded = await _staffRepository.AddStaffAccount(staffObject, branchId);
                    if (isManagerAdded)
                    {
                        message.Result = true;
                        message.ResultMessage = $"Account Created for {staffName} with Account Id:{staffAccountId}";
                        message.Data = staffAccountId;
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Failed to Create Staff Account for {staffName}";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Staff: {staffName} Already Existed";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> IsAccountExistAsync(string branchId, string staffAccountId)
        {
            Message message;
            message = await IsStaffExistAsync(branchId);
            if (message.Result)
            {
                bool isManagerExist = await _staffRepository.IsStaffExist(staffAccountId, branchId);
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

        public async Task<Message> UpdateStaffAccountAsync(string branchId, string staffAccountId, string staffName, string staffPassword, Roles staffRole)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, staffAccountId);
            if (message.Result)
            {
                Staff staff = await _staffRepository.GetStaffById(staffAccountId, branchId);
                byte[] salt = null;
                byte[] hashedPassword = null;
                bool canContinue = true;
                if (staff is not null && staffPassword is not null)
                {
                    salt = staff!.Salt;
                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(staffPassword, salt);
                    if (Convert.ToBase64String(staff.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
                    {
                        message.Result = false;
                        message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        canContinue = false;
                    }
                    salt = _encryptionService.GenerateSalt();
                    hashedPassword = _encryptionService.HashPassword(staffPassword, salt);
                }

                if (canContinue && staff is not null)
                {
                    Staff staffObject = new()
                    {
                        AccountId = staffAccountId,
                        Name = staffName,
                        HashedPassword = hashedPassword,
                        Salt = salt,
                        IsActive = true
                    };
                    bool isDetailsUpdated = await _staffRepository.UpdateStaffAccount(staffObject, branchId);
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
                message.ResultMessage = "Staff Validation Failed.";
            }
            return message;
        }

        public async Task<Message> DeleteStaffAccountAsync(string branchId, string staffAccountId)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, staffAccountId);
            if (message.Result)
            {
                bool isDeleted = await _staffRepository.DeleteStaffAccount(staffAccountId, branchId);
                if (isDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{staffAccountId} Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete Staff Account Id:{staffAccountId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Staff with Account Id:{staffAccountId} doesn't Exist.";
            }
            return message;
        }

        public async Task<Staff> GetStaffDetailsAsync(string branchId, string staffAccountId)
        {
            return await _staffRepository.GetStaffById(staffAccountId, branchId);
        }
    }
}
