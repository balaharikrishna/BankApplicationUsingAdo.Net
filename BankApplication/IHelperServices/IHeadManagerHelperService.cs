namespace BankApplication.IHelperServices
{
    public interface IHeadManagerHelperService
    {
        /// <summary>
        /// Excecutes Head Manager Actions based on selected Option.
        /// </summary>
        /// <param name="Option">Option is to Execute the Required Action.</param>
        /// <param name="headManagerBankId">Head Manager Bank Id of the Bank.</param>
        void SelectedOption(ushort Option, string headManagerBankId);
    }
}