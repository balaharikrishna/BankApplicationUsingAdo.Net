using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface IUserRepository
    {
        Task<AuthenticateUser> GetUserAuthenticationDetails(string accountId, string name);
    }
}
