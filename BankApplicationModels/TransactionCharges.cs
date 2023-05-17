using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class TransactionCharges
    {
        [Required]
        public ushort RtgsSameBank { get; set; }
        [Required]
        public ushort RtgsOtherBank { get; set; }
        [Required]
        public ushort ImpsSameBank { get; set; }
        [Required]
        public ushort ImpsOtherBank { get; set; }
        [Required]
        public bool IsActive { get; set; }
        //public override string ToString()
        //{
        //    return $"TransactionCharges: RtgsSameBank:{RtgsSameBank}% , RtgsOtherBank:{RtgsOtherBank}% , ImpsSameBank:{ImpsSameBank}% , ImpsOtherBank:{ImpsOtherBank}%";
        //}
    }


}
