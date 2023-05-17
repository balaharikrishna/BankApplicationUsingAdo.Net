using API.Models;
using API.ViewModels.Customer;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, IMapper mapper, ICustomerService customerService)
        {
            _logger = logger;
            _mapper = mapper;
            _customerService = customerService;
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("branchId/{id}")]
        public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Customers");
                IEnumerable<Customer> customers = await _customerService.GetAllCustomersAsync(id);
                if (customers is null || !customers.Any())
                {
                    return NotFound("Customers Not Found.");
                }
                List<CustomerDto> customerDtos = _mapper.Map<List<CustomerDto>>(customers);
                return Ok(customerDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Customers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Customers.");
            }
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}/accountId/{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Customer Account with id {id}");
                Customer customer = await _customerService.GetCustomerByIdAsync(branchId, id);
                if (customer is null)
                {
                    return NotFound("Customer Not Found");
                }
                CustomerDto customerDto = _mapper.Map<CustomerDto>(customer);
                return Ok(customerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Customer with id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Customer Details.");
            }
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}/name/{name}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerByName([FromRoute] string branchId, [FromRoute] string name)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Customer Account with Name {name}");
                Customer customer = await _customerService.GetCustomerByNameAsync(branchId, name);
                if (customer is null)
                {
                    return NotFound("Customer Not Found");
                }
                CustomerDto customerDto = _mapper.Map<CustomerDto>(customer);
                return Ok(customerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Customer with Name {name} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Customer Details.");
            }
        }

        [Authorize(Policy = "ManagerStaffOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> OpenCustomerAccount([FromBody] AddCustomerViewModel customerViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Creating Customer Account");
                Message message = await _customerService.OpenCustomerAccountAsync(customerViewModel.BranchId, customerViewModel.CustomerName,
                customerViewModel.CustomerPassword, customerViewModel.CustomerPhoneNumber, customerViewModel.CustomerEmailId, customerViewModel.CustomerAccountType,
                customerViewModel.CustomerAddress, customerViewModel.CustomerDateOfBirth, customerViewModel.CustomerGender);
                if (message.Result)
                {
                    return Created($"{Request.Path}/accountId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Creating a new Customer Account Failed");
                    return BadRequest($"An error occurred while creating an Account.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating a new Customer Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating an Account.");
            }
        }

        [Authorize(Policy = "ManagerStaffOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateCustomerAccount([FromBody] UpdateCustomerViewModel updateCustomerViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Updating Customer with Id {updateCustomerViewModel.CustomerAccountId}");
                Message message = await _customerService.UpdateCustomerAccountAsync(updateCustomerViewModel.BranchId, updateCustomerViewModel.CustomerAccountId, updateCustomerViewModel.CustomerName,
                updateCustomerViewModel.CustomerPassword, updateCustomerViewModel.CustomerPhoneNumber, updateCustomerViewModel.CustomerEmailId, updateCustomerViewModel.CustomerAccountType,
                updateCustomerViewModel.CustomerAddress, updateCustomerViewModel.CustomerDateOfBirth, updateCustomerViewModel.CustomerGender);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Customer Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Branch with Id {updateCustomerViewModel.CustomerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Customer Details.");
            }
        }

        [Authorize(Policy = "ManagerStaffOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{branchId}/accountId/{id}")]
        public async Task<ActionResult<Message>> DeleteCustomerAccount([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Customer Account with Id {id}");
                Message message = await _customerService.DeleteCustomerAccountAsync(branchId, id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Customer Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Customer Account with Id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Customer Account.");
            }
        }

        [Authorize(Policy = "CustomerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("deposit")]
        public async Task<ActionResult<Message>> DepositAmount([FromBody] DepositViewModel depositViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Adding Amount In Account with Id {depositViewModel.AccountId}");
                Message message = await _customerService.DepositAmountAsync(depositViewModel.BankId, depositViewModel.BranchId, depositViewModel.AccountId,
                    depositViewModel.DepositAmount, depositViewModel.CurrencyCode);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Customer Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Adding Amount In Account with Id {depositViewModel.AccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Adding the Account.");
            }
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("balance/{branchId}/accountId/{id}")]
        public async Task<ActionResult<Message>> GetAccountBalance([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Customer Balance with Id {id}");
                Message message = await _customerService.CheckAccountBalanceAsync(branchId, id);
                if (!message.Result)
                {
                    return NotFound(message.ResultMessage);
                }
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Customer Balance with Id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Customer Balance.");
            }
        }

        [Authorize(Policy = "CustomerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("withdraw")]
        public async Task<ActionResult<Message>> WithdrawAmount([FromBody] WithDrawViewModel withDrawViewModel)
        {
            try
            {
               
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Withdraw Customer Balance with Id {withDrawViewModel.AccountId}");
                Message message = await _customerService.WithdrawAmountAsync(withDrawViewModel.BankId, withDrawViewModel.BranchId,
                    withDrawViewModel.AccountId, withDrawViewModel.withDrawAmount);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Customer Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Withdraw Customer Balance with Id {withDrawViewModel.AccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Withdraw the Customer Balance.");
            }
        }

        [Authorize(Policy = "CustomerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("transfer")] 
        public async Task<ActionResult<Message>> TransferAmount([FromBody] TransferAmountViewModel transferAmountViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Transfering Customer Balance with Account Id {transferAmountViewModel.AccountId}");
                Message message = await _customerService.TransferAmountAsync(transferAmountViewModel.BankId, transferAmountViewModel.BranchId,
                    transferAmountViewModel.AccountId, transferAmountViewModel.ToBankId, transferAmountViewModel.ToBranchId, transferAmountViewModel.ToAccountId,
                    transferAmountViewModel.TransferAmount, transferAmountViewModel.TransferMethod);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Customer Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Transfering Amount with Account Id {transferAmountViewModel.AccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Transfering the Amount.");
            }
        }
    }
}
