using BankApplicationModels.Enums;

namespace API.Models
{
    public class CustomerDto : HeadManagerDto
    {
        public decimal Balance { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailId { get; set; }

        public AccountType AccountType { get; set; }

        public string? Address { get; set; }
        public string? DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string? PassbookIssueDate { get; set; }
    }
}
