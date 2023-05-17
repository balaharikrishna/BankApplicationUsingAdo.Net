using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Customer : HeadManager
    {
        [Required]
        public decimal Balance { get; set; }

        [Required]
        [RegularExpression("^\\d{10}$")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string EmailId { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        public string Address { get; set; }
        [RegularExpression(@"^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/(\d{4})$")]

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string PassbookIssueDate { get; set; }

        public new Roles Role = Roles.Customer;
        //public List<Transactions> Transactions { get; set; }
        //public override string ToString()
        //{
        //    return $"\nAccountID: {AccountId}  Name:{Name}  Avl.Bal:{Balance}  PhoneNumber:{PhoneNumber}\nEmailId:{EmailId}  AccountType:{AccountType}  Address:{Address}  DateOfBirth:{DateOfBirth}\nGender:{Gender}  PassbookIssueDate:{PassbookIssueDate}\n";
        //}
    }
}
