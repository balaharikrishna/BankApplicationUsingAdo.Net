namespace BankApplication.IHelperServices
{
    public interface IManagerHelperService
    {
        /// <summary>
        /// Excecutes Manager Actions based on selected Option.
        /// </summary>
        /// <param name="Option">Option is to Execute the Required Action.</param>
        /// <param name="managerBankId">BankId of the Bank</param>
        /// <param name="managerBranchId">BranhId of the Branch</param>
        /// <param name="managerAccountId">AccountId of the Manager</param>
        void SelectedOption(ushort Option, string managerBankId, string managerBranchId);
    }
}