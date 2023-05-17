using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface IReserveBankManagerRepository
    {
        Task<IEnumerable<ReserveBankManager>> GetAllReserveBankManagers();
        Task<bool> AddReserveBankManager(ReserveBankManager reserveBankManager);
        Task<bool> UpdateReserveBankManager(ReserveBankManager reserveBankManager);
        Task<bool> DeleteReserveBankManager(string reserveBankManagerAccountId);
        Task<bool> IsReserveBankManagerExist(string reserveBankManagerAccountId);
        Task<ReserveBankManager?> GetReserveBankManagerById(string reserveBankManagerAccountId);
        Task<ReserveBankManager?> GetReserveBankManagerByName(string reserveBankManagerName);
    }
}
