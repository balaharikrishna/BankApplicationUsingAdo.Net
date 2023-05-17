namespace API.ViewModels.TransactionCharges
{
    public class TransactionChargesViewModel
    {
        public string BranchId { get; set; }
        public ushort RtgsSameBank { get; set; }

        public ushort RtgsOtherBank { get; set; }

        public ushort ImpsSameBank { get; set; }

        public ushort ImpsOtherBank { get; set; }
    }
}
