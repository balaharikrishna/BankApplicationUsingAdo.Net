using API.Models;
using API.ViewModels.ReserveBankManager;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = "ReserveBankManagerOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveBankManagerController : ControllerBase
    {
        private readonly ILogger<ReserveBankManagerController> _logger;
        private readonly IMapper _mapper;
        private readonly IReserveBankManagerService _reserveBankManagerService;

        public ReserveBankManagerController(ILogger<ReserveBankManagerController> logger, IMapper mapper, IReserveBankManagerService reserveBankManagerService)
        {
            _logger = logger;
            _mapper = mapper;
            _reserveBankManagerService = reserveBankManagerService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<ReserveBankManagerDto>>> GetAllReserveBankManagers()
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Reserve Bank Managers");
                IEnumerable<ReserveBankManager> reserveBankManagers = await _reserveBankManagerService.GetAllReserveBankManagersAsync();
                if (reserveBankManagers is null || !reserveBankManagers.Any())
                {
                    return NotFound("Reserve Bank Managers Not Found.");
                }
                List<ReserveBankManagerDto> reserveBankManagerDtos = _mapper.Map<List<ReserveBankManagerDto>>(reserveBankManagers);
                return Ok(reserveBankManagerDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Reserve Bank Managers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Reserve Bank Managers.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("accountId/{id}")]
        public async Task<ActionResult<ReserveBankManagerDto>> GetReserveBankManagerById([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Reserve Bank Manager Account with Id {id}");
                ReserveBankManager reserveBankManager = await _reserveBankManagerService.GetReserveBankManagerByIdAsync(id);
                if (reserveBankManager is null)
                {
                    return NotFound("Reserve Bank Manager Not Found");
                }
                ReserveBankManagerDto reserveBankManagerDto = _mapper.Map<ReserveBankManagerDto>(reserveBankManager);
                return Ok(reserveBankManagerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Reserve Bank Manager with id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Reserve Bank Manager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("accountName/name/{name}")]
        public async Task<ActionResult<ReserveBankManagerDto>> GetReserveBankManagerByName([FromRoute] string name)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Reserve Bank Manager Account with Name {name}");
                ReserveBankManager reserveBankManager = await _reserveBankManagerService.GetReserveBankManagerByNameAsync(name);
                if (reserveBankManager is null)
                {
                    return NotFound("Reserve Bank Manager Not Found.");
                }
                ReserveBankManagerDto reserveBankManagerDto = _mapper.Map<ReserveBankManagerDto>(reserveBankManager);
                return Ok(reserveBankManagerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Reserve Bank Manager with Name {name} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Reserve Bank Manager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> OpenReserveBankManagerAccount([FromBody] AddReserveBankManagerAccountViewModel addReserveBankManagerAccountViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Opening Reserve Bank Manager Account");
                Message message = await _reserveBankManagerService.OpenReserveBankManagerAccountAsync(addReserveBankManagerAccountViewModel.ReserveBankManagerName!,
                addReserveBankManagerAccountViewModel.ReserveBankManagerPassword!);
                if (message.Result)
                {
                    return Created($"{Request.Path}/accountId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Opening a new Reserve Bank Manager Account Failed");
                    return BadRequest($"An error occurred while creating Reserve Bank Manager Account.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Opening a new Reserve Bank Manager Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating Reserve Bank Manager Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateReserveBankManagerAccount([FromBody] UpdateReserveBankManagerAccountViewModel updateReserveBankManagerAccount)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.Log(LogLevel.Information, message: $"Updating Reserve Bank Manager Account with Id {updateReserveBankManagerAccount.ReserveBankManagerAccountId}");
                Message message = await _reserveBankManagerService.UpdateReserveBankManagerAccountAsync(updateReserveBankManagerAccount.ReserveBankManagerAccountId!,
                    updateReserveBankManagerAccount.ReserveBankManagerName!, updateReserveBankManagerAccount.ReserveBankManagerPassword!);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Reserve Bank Manager Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Reserve Bank Manager Account Account with Id {updateReserveBankManagerAccount.ReserveBankManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Reserve Bank Manager Account Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("accountId/{id}")]
        public async Task<ActionResult<Message>> DeleteReserveBankManagerAccount([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Reserve Bank Manager Account Account with Id {id}");
                Message message = await _reserveBankManagerService.DeleteReserveBankManagerAccountAsync(id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Reserve Bank Manager Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Reserve Bank Manager Account Account with Id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Reserve Bank Manager Account.");
            }
        }
    }
}

