using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Bank
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public string BankName { get; set; }
        [Required]
        public string BankId { get; set; }
        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }
        //public List<Branch> Branches { get; set; }
        //public List<HeadManager> HeadManagers { get; set; }
        //public List<Currency> Currency { get; set; }
    }
}
