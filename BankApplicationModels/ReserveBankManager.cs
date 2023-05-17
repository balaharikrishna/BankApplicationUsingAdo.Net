using BankApplicationModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels
{
    public class ReserveBankManager : HeadManager
    {
        public new Roles Role = Roles.ReserveBankManager;
    }
}
