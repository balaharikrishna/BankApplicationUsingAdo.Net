namespace BankApplication.IHelperServices
{
    public interface IReserveBankManagerHelperService
    {
        /// <summary>
        /// Excecutes Reserve Bank Manager Actions based on selected Option.
        /// </summary>
        /// <param name="Option">Option is to Execute the Required Action.</param>
        void SelectedOption(ushort Option);
    }
}