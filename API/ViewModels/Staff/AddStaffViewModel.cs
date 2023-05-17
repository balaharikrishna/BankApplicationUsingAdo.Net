using BankApplicationModels.Enums;

namespace API.ViewModels.Staff
{
    public class AddStaffViewModel
    {
        public string? BranchId { get; set; }
        public string? StaffName { get; set; }
        public string? StaffPassword { get; set; }
        public Roles StaffRole { get; set; }
    }
}
