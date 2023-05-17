namespace BankApplication.IHelperServices
{
    public interface IStaffHelperService
    {
        /// <summary>
        /// Excecutes Staff Actions based on selected Option.
        /// </summary>
        /// <param name="Option">Option is to Execute the Required Action.</param>
        /// <param name="staffBankId">BankId of the Bank</param>
        /// <param name="staffBranchId">BranhId of the Branch</param>
        void SelectedOption(ushort Option, string staffBankId, string staffBranchId);
    }
}