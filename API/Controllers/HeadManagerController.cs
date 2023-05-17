using API.Models;
using API.ViewModels.HeadManager;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeadManagerController : ControllerBase
    {
        private readonly ILogger<HeadManagerController> _logger;
        private readonly IMapper _mapper;
        private readonly IHeadManagerService _headManagerService;

        public HeadManagerController(ILogger<HeadManagerController> logger, IMapper mapper, IHeadManagerService headManagerService)
        {
            _logger = logger;
            _mapper = mapper;
            _headManagerService = headManagerService;
        }

        [Authorize(Policy = "MinimumHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("bankId/{id}")]
        public async Task<ActionResult<List<HeadManagerDto>>> GetAllHeadManagers([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching all HeadManagers");
                IEnumerable<HeadManager> headManagers = await _headManagerService.GetAllHeadManagersAsync(id);
                if (headManagers is null || !headManagers.Any())
                {
                    return NotFound("Managers Not Found.");
                }
                List<HeadManagerDto> headManagerDtos = _mapper.Map<List<HeadManagerDto>>(headManagers);
                return Ok(headManagerDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the HeadManagers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the HeadManagers.");
            }
        }

        [Authorize(Policy = "MinimumHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{bankId}/accountId/{id}")]
        public async Task<ActionResult<HeadManagerDto>> GetHeadManagerById([FromRoute] string bankId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching HeadManager Account with id {id}");
                HeadManager headManager = await _headManagerService.GetHeadManagerByIdAsync(bankId, id);
                if (headManager is null)
                {
                    return NotFound("Head Manager Not Found");
                }
                HeadManagerDto headManagerDto = _mapper.Map<HeadManagerDto>(headManager);
                return Ok(headManagerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching HeadManager Account with id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the headManager Details.");
            }
        }

        [Authorize(Policy = "MinimumHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{bankId}/name/{name}")]
        public async Task<ActionResult<HeadManagerDto>> GetHeadManagerByName([FromRoute] string bankId, [FromRoute] string name)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching headManager Account with Name {name}");
                HeadManager headManager = await _headManagerService.GetHeadManagerByNameAsync(bankId, name);
                if (headManager is null)
                {
                    return NotFound("Head Manager Not Found");
                }
                HeadManagerDto headManagerDto = _mapper.Map<HeadManagerDto>(headManager);
                return Ok(headManagerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching headManager Account with Name {name} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Head Manager Details.");
            }
        }

        [Authorize(Policy = "ReserveBankManagerOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> OpenHeadManagerAccount([FromBody] AddHeadManagerViewModel HeadManagerViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Creating HeadManager Account");
                Message message = await _headManagerService.OpenHeadManagerAccountAsync(HeadManagerViewModel.BankId, HeadManagerViewModel.HeadManagerName, HeadManagerViewModel.HeadManagerPassword);
                if (message.Result)
                {
                    return Created($"{Request.Path}/accountId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Creating HeadManager Account Failed");
                    return BadRequest($"An error occurred while creating HeadManager Account.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating HeadManager Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating Head Manager Account.");
            }
        }

        [Authorize(Policy = "ReserveBankManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateHeadManagerAccount([FromBody] UpdateHeadManagerViewModel updateHeadManagerViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Updating HeadManager with Account Id {updateHeadManagerViewModel.HeadManagerAccountId}");
                Message message = await _headManagerService.UpdateHeadManagerAccountAsync(updateHeadManagerViewModel.BankId, updateHeadManagerViewModel.HeadManagerAccountId,
                updateHeadManagerViewModel.HeadManagerName, updateHeadManagerViewModel.HeadManagerPassword);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Head Manager Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating HeadManager with Account Id {updateHeadManagerViewModel.HeadManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Head Manager Account.");
            }
        }

        [Authorize(Policy = "ReserveBankManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{branchId}/accountId/{id}")]
        public async Task<ActionResult<Message>> DeleteHeadManagerAccount([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting HeadManager Account with Id {id}");
                Message message = await _headManagerService.DeleteHeadManagerAccountAsync(branchId, id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Head Manager Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting HeadManager Account with Id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the HeadManager Account.");
            }
        }
    }
}
