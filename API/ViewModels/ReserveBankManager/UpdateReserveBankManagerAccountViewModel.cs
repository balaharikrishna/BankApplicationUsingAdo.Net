using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.ReserveBankManager
{
    public class UpdateReserveBankManagerAccountViewModel : AddReserveBankManagerAccountViewModel
    {
        [Required]
        public string? ReserveBankManagerAccountId { get; set; }
    }
}
