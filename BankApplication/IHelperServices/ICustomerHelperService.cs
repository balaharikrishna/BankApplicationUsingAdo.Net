namespace BankApplication.IHelperServices
{
    public interface ICustomerHelperService
    {
        /// <summary>
        /// Excecutes Customer Actions based on selected Option.
        /// </summary>
        /// <param name="Option">Option is to Execute the Required Action.</param>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="branchId">BranhId of the Bank</param>
        /// <param name="accountId">AccountId of the Customer</param>
        void SelectedOption(ushort Option, string bankId, string branchId, string accountId);
    }
}