using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankApplicationServices.Services
{
    public class TokenIssueService : ITokenIssueService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public TokenIssueService(IEncryptionService encryptionService, IUserRepository userRepository, IConfiguration configuration)
        {
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<Message> IssueToken(string accountId,string userName, string password)
        {
            Message message = new();
            AuthenticateUser user = await _userRepository.GetUserAuthenticationDetails(accountId, userName);
            if (user is not null)
            {
                byte[] salt = user.Salt;
                byte[] hashedPasswordToCheck = _encryptionService.HashPassword(password, salt);
                bool isValidPassword = Convert.ToBase64String(user!.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                if (isValidPassword)
                {
                    string token = await GenerateTokenAsync(user.AccountId,user.Name, user.Role);
                    message.Result = true;
                    message.ResultMessage = "User Login Successful.";
                    message.Data = token;
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "User Login Failed.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "User Login Failed.";
            }
            return message;
        }

        public async Task<string> GenerateTokenAsync(string accountId,string name, Roles role)
        {
            SymmetricSecurityKey mySecurityKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                  new Claim("AccountId", accountId),
                  new Claim("Name", name),
                  new Claim("Role", role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = await Task.Run(() => tokenHandler.CreateToken(tokenDescriptor));
            return tokenHandler.WriteToken(token);
        }
    }
}
