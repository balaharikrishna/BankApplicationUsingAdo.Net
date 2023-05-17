using BankApplicationModels.Enums;

namespace API.Models
{
    public class HeadManagerDto 
    {
        public string Name { get; set; }

        public string AccountId { get; set; }
        public Roles Role { get; set; }
    }
}
