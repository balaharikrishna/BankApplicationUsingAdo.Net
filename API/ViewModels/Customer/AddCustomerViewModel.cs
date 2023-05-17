using BankApplicationModels.Enums;

namespace API.ViewModels.Customer
{
    public class AddCustomerViewModel
    {
        public string? BranchId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPassword { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerEmailId { get; set; }
        public AccountType CustomerAccountType { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerDateOfBirth { get; set; }
        public Gender CustomerGender { get; set; }
    }
}
