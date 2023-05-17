using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IReserveBankManagerService
    {
        Task<IEnumerable<ReserveBankManager>> GetAllReserveBankManagersAsync();
        Task<ReserveBankManager> GetReserveBankManagerByIdAsync(string reserveBankManagerAccountId);
        Task<ReserveBankManager> GetReserveBankManagerByNameAsync(string reserveBankManagerName);
        Task<Message> AuthenticateManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerPassword);
        Task<Message> IsReserveBankManagersExistAsync();
        Task<Message> OpenReserveBankManagerAccountAsync(string ReserveBankManagerName, string ReserveBankManagerPassword);
        Task<Message> UpdateReserveBankManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerName, string ReserveBankManagerPassword);
        Task<Message> DeleteReserveBankManagerAccountAsync(string ReserveBankManagerAccountId);
        Task<Message> IsAccountExistAsync(string ReserveBankManagerAccountId);
    }
}