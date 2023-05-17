using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class AuthenticateUserDto
    {
        public string AccountId { get; set; }
        public string Name { get; set; }
        public Roles Role { get; set; }
    }
}
