using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ITokenIssueService
    {
        /// <summary>
        /// authenticates the given userName and password.
        /// </summary>
        /// <param name="userName">user Name of User.</param>
        /// <param name="password">password of User.</param>
        /// <returns>A Message object containing information about the success or failure of the operation.</returns>
        Task<Message> IssueToken(string accountId, string userName, string password);
    }
}
