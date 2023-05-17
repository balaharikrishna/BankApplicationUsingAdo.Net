using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class AuthenticateUser
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public byte[] Salt { get; set; }

        [Required]
        public byte[] HashedPassword { get; set; }

        [Required]
        public string AccountId { get; set; }

        [Required]
        public Roles Role { get; set; }
    }

}
