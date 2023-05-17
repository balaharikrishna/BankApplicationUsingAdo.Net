using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Branch
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public string BranchName { get; set; }
        [Required]
        public string BranchId { get; set; }
        [Required]
        public string BranchAddress { get; set; }
        [Required]
        [RegularExpression("^\\d{10}$")]
        public string BranchPhoneNumber { get; set; }
        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }
        //public List<Manager> Managers { get; set; }
        //public List<TransactionCharges> Charges { get; set; }
        //public List<Staff> Staffs { get; set; }
        //public List<Customer> Customers { get; set; }
    }
}
