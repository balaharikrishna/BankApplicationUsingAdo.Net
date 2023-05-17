using Microsoft.Build.Framework;

namespace API.ViewModels.ReserveBankManager
{
    public class AddReserveBankManagerAccountViewModel
    {
        [Required]
        public string? ReserveBankManagerName { get; set; }
        [Required]
        public string? ReserveBankManagerPassword { get; set; }
    }
}
