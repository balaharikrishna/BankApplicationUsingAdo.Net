using API.Models;
using API.ViewModels.Manager;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly IMapper _mapper;
        private readonly IManagerService _managerService;

        public ManagerController(ILogger<ManagerController> logger, IMapper mapper, IManagerService managerService)
        {
            _logger = logger;
            _mapper = mapper;
            _managerService = managerService;
        }

        [Authorize(Policy = "ManagerHeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("branchId/{id}")]
        public async Task<ActionResult<List<ManagerDto>>> GetAllManagers([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Managers");
                IEnumerable<Manager> Managers = await _managerService.GetAllManagersAsync(id);
                if (Managers is null || !Managers.Any())
                {
                    return NotFound("Managers Not Found.");
                }
                List<ManagerDto> managerDtos = _mapper.Map<List<ManagerDto>>(Managers);
                return Ok(managerDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Managers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Managers.");
            }
        }

        [Authorize(Policy = "ManagerHeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}/accountId/{id}")]
        public async Task<ActionResult<ManagerDto>> GetManagerById([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching manager Account with id {id}");
                Manager manager = await _managerService.GetManagerByIdAsync(branchId, id);
                if (manager is null)
                {
                    return NotFound("Manager Not Found");
                }
                ManagerDto managerDto = _mapper.Map<ManagerDto>(manager);
                return Ok(managerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching manager with id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the manager Details.");
            }
        }

        [Authorize(Policy = "ManagerHeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}/name/{name}")]
        public async Task<ActionResult<ManagerDto>> GetManagerByName([FromRoute] string branchId, [FromRoute] string name)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Manager Account with Name {name}");
                Manager manager = await _managerService.GetManagerByNameAsync(branchId, name);
                if (manager is null)
                {
                    return NotFound("Manager Not Found");
                }
                ManagerDto managerDto = _mapper.Map<ManagerDto>(manager);
                return Ok(managerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching manager with Name {name} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the manager Details.");
            }
        }

        [Authorize(Policy = "HeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> OpenManagerAccount([FromBody] AddManagerViewModel managerViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Opening manager Account");
                Message message = await _managerService.OpenManagerAccountAsync(managerViewModel.BranchId, managerViewModel.ManagerName,
                managerViewModel.ManagerPassword);
                if (message.Result)
                {
                    return Created($"{Request.Path}/accountId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Opening a new manager Account Failed");
                    return BadRequest($"An error occurred while creating manager Account.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Opening a new manager Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating an Account.");
            }
        }

        [Authorize(Policy = "HeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateManagerAccount([FromBody] UpdateManagerViewModel updatemanagerViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Updating manager with Id {updatemanagerViewModel.ManagerAccountId}");
                Message message = await _managerService.UpdateManagerAccountAsync(updatemanagerViewModel.BranchId, updatemanagerViewModel.ManagerAccountId,
                    updatemanagerViewModel.ManagerName, updatemanagerViewModel.ManagerPassword);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Manager Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Manager Account with Id {updatemanagerViewModel.ManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Manager Account.");
            }
        }

        [Authorize(Policy = "HeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{branchId}/accountId/{id}")]
        public async Task<ActionResult<Message>> DeleteManagerAccount([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting manager Account with Id {id}");
                Message message = await _managerService.DeleteManagerAccountAsync(branchId, id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Manager Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting manager Account with Id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the manager Account.");
            }
        }
    }
}
