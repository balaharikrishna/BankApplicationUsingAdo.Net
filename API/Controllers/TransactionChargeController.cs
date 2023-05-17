using API.Models;
using API.ViewModels.TransactionCharges;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionChargeController : ControllerBase
    {
        private readonly ILogger<TransactionChargeController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionChargeService _transactionChargeService;

        public TransactionChargeController(ILogger<TransactionChargeController> logger, IMapper mapper, ITransactionChargeService transactionChargeService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionChargeService = transactionChargeService;
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("branchId/{id}")]
        public async Task<ActionResult<List<TransactionChargesDto>>> GetTransactionCharges([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching Transaction Charges");
                TransactionCharges transactionCharges = await _transactionChargeService.GetTransactionCharges(id);
                if (transactionCharges is null)
                {
                    return NotFound("Transaction Charges Not Available");
                }
                TransactionChargesDto transactionChargesDto = _mapper.Map<TransactionChargesDto>(transactionCharges);
                return Ok(transactionChargesDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Transaction Charges.");
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> AddTransactionCharges([FromBody] TransactionChargesViewModel transactionChargesViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Adding new Transaction Charges");
                Message message = await _transactionChargeService.AddTransactionChargesAsync(transactionChargesViewModel.BranchId, transactionChargesViewModel.RtgsSameBank,
                transactionChargesViewModel.RtgsOtherBank, transactionChargesViewModel.ImpsSameBank, transactionChargesViewModel.ImpsOtherBank);
                if (message.Result)
                {
                    return Created($"{Request.Path}/charges/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Adding new Transaction Charges Failed");
                    return BadRequest($"An error occurred while Adding Transaction Charges.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Adding new Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Transaction Charges.");
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateTransactionCharges([FromBody] TransactionChargesViewModel transactionChargesViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Updating Transaction Charges");
                Message message = await _transactionChargeService.UpdateTransactionChargesAsync(transactionChargesViewModel.BranchId, transactionChargesViewModel.RtgsSameBank,
                transactionChargesViewModel.RtgsOtherBank, transactionChargesViewModel.ImpsSameBank, transactionChargesViewModel.ImpsOtherBank);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Transaction Charges Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Transaction Charges.");
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("branchId/{id}")]
        public async Task<ActionResult<Message>> DeleteTransactionCharges([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Transaction Charges");
                Message message = await _transactionChargeService.DeleteTransactionChargesAsync(id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Transaction Charges Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Transaction Charges.");
            }
        }
    }
}
