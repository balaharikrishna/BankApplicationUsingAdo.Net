using API.ViewModels;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ITokenIssueService _tokenIssueService;

        public LoginController(ILogger<CustomerController> logger, ITokenIssueService tokenIssueService)
        {
            _logger = logger;
            _tokenIssueService = tokenIssueService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<string>> UserLogin([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Authenticating UserName and Password");
                Message message = await _tokenIssueService.IssueToken(loginViewModel.AccountId, loginViewModel.UserName, loginViewModel.Password);
                if (message.Result)
                {
                    return Ok(message.Data);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Unauthorized");
                    return Unauthorized($"Unauthorized.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Authentication Failed");
                return Unauthorized("Unauthorized");
            }
        }
    }
}
