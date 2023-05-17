using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Staff : HeadManager
    {
        public new Roles Role = Roles.Staff;
    }
}
