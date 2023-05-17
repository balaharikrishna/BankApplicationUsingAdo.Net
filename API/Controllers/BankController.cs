using API.Models;
using API.ViewModels.Bank;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = "ReserveBankManagerOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;
        private readonly IMapper _mapper;
        private readonly IBankService _bankService;

        public BankController(ILogger<BankController> logger, IMapper mapper, IBankService bankService)
        {
            _logger = logger;
            _mapper = mapper;
            _bankService = bankService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<BankDto>>> GetAllBanks()
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the banks");
                IEnumerable<Bank> banks = await _bankService.GetAllBanksAsync();
                if (banks is null || !banks.Any())
                {
                    return NotFound("Banks Not Found.");
                }
                List<BankDto> bankDtos = _mapper.Map<List<BankDto>>(banks);
                return Ok(bankDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the banks failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the banks.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("bankId/{id}")]
        public async Task<ActionResult<BankDto>> GetBankById([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching bank with id {id}");
                Bank bank = await _bankService.GetBankByIdAsync(id);
                if (bank is null)
                {
                    return NotFound("Bank not Found");
                }
                BankDto bankDto = _mapper.Map<BankDto>(bank);
                return Ok(bankDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching bank with id {id} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("bankName/{name}")]
        public async Task<ActionResult<BankDto>> GetBankByName([FromRoute] string name)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching bank with name {name}");
                Bank bank = await _bankService.GetBankByNameAsync(name);
                if (bank is null)
                {
                    return NotFound("Bank not Found");
                }
                BankDto bankDto = _mapper.Map<BankDto>(bank);
                return Ok(bankDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching bank with name {name} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> CreateBank([FromBody] AddBankViewModel addBankViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Creating a new bank");
                Message message = await _bankService.CreateBankAsync(addBankViewModel.BankName);
                if (message.Result)
                {
                    return Created($"{Request.Path}/bankId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Creating a new bank failed");
                    return BadRequest($"An error occurred while creating Bank.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating a new bank failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateBank([FromBody] UpdateBankViewModel updateBankViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Updating bank with id {updateBankViewModel.BankId}");
                Message message = await _bankService.UpdateBankAsync(updateBankViewModel.BankId, updateBankViewModel.BankName);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Bank Not Found.");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating bank with id {updateBankViewModel.BankId} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("bankId/{id}")]
        public async Task<ActionResult<Message>> DeleteBank([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting bank with id {id}");
                Message message = await _bankService.DeleteBankAsync(id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Bank Not Found.");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting bank with id {id} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the bank.");
            }
        }
    }
}
