using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllStaffs(string branchId);
        Task<bool> AddStaffAccount(Staff staff, string branchId);
        Task<bool> UpdateStaffAccount(Staff staff, string branchId);
        Task<bool> DeleteStaffAccount(string staffAccountId, string branchId);
        Task<bool> IsStaffExist(string staffAccountId, string branchId);
        Task<Staff?> GetStaffById(string staffAccountId, string branchId);
        Task<Staff?> GetStaffByName(string staffName, string branchId);
    }
}
