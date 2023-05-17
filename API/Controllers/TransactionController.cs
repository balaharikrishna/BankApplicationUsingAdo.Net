using API.Models;
using API.ViewModels.Transactions;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, IMapper mapper, ITransactionService transactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("accountId/{id}")]
        public async Task<ActionResult<List<TransactionDto>>> GetAllTransactions([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching All Transactions");
                IEnumerable<Transaction> transactions = await _transactionService.GetAllTransactionHistory(id);
                if (transactions is null || !transactions.Any())
                {
                    return NotFound("Transactions Not Found.");
                }
                List<TransactionDto> transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);
                return Ok(transactionDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Transactions Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An Error occurred while fetching the Transactions.");
            }
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{accountId}/transactionId/{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById([FromRoute] string accountId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Transaction");
                Transaction transaction = await _transactionService.GetTransactionById(accountId, id);
                if (transaction is null)
                {
                    return NotFound("Transaction Not Found.");
                }
                TransactionDto transactionDto = _mapper.Map<TransactionDto>(transaction);
                return Ok(transactionDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Transaction Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An Error occurred while fetching the Transaction.");
            }
        }

        [Authorize(Policy = "CustomerOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> AddCustomerTransaction([FromBody] AddCustomerTransactionViewModel addCustomerTransactionViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Adding new Transaction");
                Message message = await _transactionService.TransactionHistoryAsync(addCustomerTransactionViewModel.CustomerBankId, addCustomerTransactionViewModel.CustomerBranchId,
                    addCustomerTransactionViewModel.CustomerAccountId,addCustomerTransactionViewModel.Debit, addCustomerTransactionViewModel.Credit,
                addCustomerTransactionViewModel.Balance, addCustomerTransactionViewModel.TransactionType);
                if (message.Result)
                {
                    return Created($"{Request.Path}/transactionId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Adding new Transaction Failed");
                    return BadRequest($"An error occurred while Adding Transaction.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Adding new Transaction Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Adding the Transaction.");
            }
        }

        [Authorize(Policy = "CustomerOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("fromAndToTransaction")]
        public async Task<ActionResult<Message>> AddFromAndToCustomerTransaction([FromBody] AddFromAndToCustomerTransactionViewModel addFromAndToCustomerTransactionViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Adding new Transaction");
                Message message = await _transactionService.TransactionHistoryFromAndToAsync(addFromAndToCustomerTransactionViewModel.FromCustomerBankId,
                addFromAndToCustomerTransactionViewModel.FromCustomerBranchId, addFromAndToCustomerTransactionViewModel.FromCustomerAccountId,
                addFromAndToCustomerTransactionViewModel.ToCustomerBankId, addFromAndToCustomerTransactionViewModel.ToCustomerBranchId,
                addFromAndToCustomerTransactionViewModel.ToCustomerAccountId, addFromAndToCustomerTransactionViewModel.Debit,
                addFromAndToCustomerTransactionViewModel.Credit, addFromAndToCustomerTransactionViewModel.Balance, addFromAndToCustomerTransactionViewModel.ToCustomerBalance,
                addFromAndToCustomerTransactionViewModel.TransactionType);
                if (message.Result)
                {
                    return Created($"{Request.Path}/transactionId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Adding new Transaction Failed");
                    return BadRequest($"An error occurred while Adding Transaction.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Adding new Transaction Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Adding the Transaction.");
            }
        }

        [Authorize(Policy = "ManagerStaffOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("revert")]
        public async Task<ActionResult<Message>> RevertTransaction([FromBody] RevertTransactionViewModel revertTransactionViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Reverting Transaction.");
                Message message = await _transactionService.RevertTransactionAsync(revertTransactionViewModel.TransactionId, revertTransactionViewModel.FromCustomerBankId,
                revertTransactionViewModel.FromCustomerBranchId, revertTransactionViewModel.FromCustomerAccountId, revertTransactionViewModel.ToCustomerBankId,
                revertTransactionViewModel.ToCustomerBranchId, revertTransactionViewModel.ToCustomerAccountId);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Transaction Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Reverting Transaction Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Reverting Transaction.");
            }
        }
    }
}
