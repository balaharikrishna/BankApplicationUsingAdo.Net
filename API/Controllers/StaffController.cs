using API.Models;
using API.ViewModels.Staff;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IMapper _mapper;
        private readonly IStaffService _StaffService;

        public StaffController(ILogger<StaffController> logger, IMapper mapper, IStaffService StaffService)
        {
            _logger = logger;
            _mapper = mapper;
            _StaffService = StaffService;
        }

        [Authorize(Policy = "ManagerStaffOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("branchId/{id}")]
        public async Task<ActionResult<List<Staff>>> GetAllStaffs([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Staffs");
                IEnumerable<Staff> staffs = await _StaffService.GetAllStaffAsync(id);
                if (staffs is null || !staffs.Any())
                {
                    return NotFound("Reserve Bank Managers Not Found.");
                }
                List<StaffDto> StaffDtos = _mapper.Map<List<StaffDto>>(staffs);
                return Ok(StaffDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Staffs Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Staff.");
            }
        }

        [Authorize(Policy = "ManagerStaffOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}/accountId/{id}")]
        public async Task<ActionResult<StaffDto>> GetStaffById([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Staff Account with id {id}");
                Staff Staff = await _StaffService.GetStaffByIdAsync(branchId, id);
                if (Staff is null)
                {
                    return NotFound("Staff Not Found");
                }
                StaffDto StaffDto = _mapper.Map<StaffDto>(Staff);
                return Ok(StaffDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Staff with id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Staff Details.");
            }
        }

        [Authorize(Policy = "ManagerStaffOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}/name/{name}")]
        public async Task<ActionResult<StaffDto>> GetStaffByName([FromRoute] string branchId, [FromRoute] string name)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Staff Account with Name {name}");
                Staff Staff = await _StaffService.GetStaffByNameAsync(branchId, name);
                if (Staff is null)
                {
                    return NotFound("Staff Not Found");
                }
                StaffDto StaffDto = _mapper.Map<StaffDto>(Staff);
                return Ok(StaffDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Staff with Name:{name} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Staff Details.");
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> OpenStaffAccount([FromBody] AddStaffViewModel staffViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Opening Staff Account");
                Message message = await _StaffService.OpenStaffAccountAsync(staffViewModel.BranchId, staffViewModel.StaffName,
                staffViewModel.StaffPassword);
                if (message.Result)
                {
                    return Created($"{Request.Path}/accountId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Opening a new Staff Account Failed");
                    return BadRequest($"An error occurred while creating Staff Account.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Opening a new Staff Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating an Account.");
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateStaffAccount([FromBody] UpdateStaffViewModel updateStaffViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.Log(LogLevel.Information, message: $"Updating Staff with Id {updateStaffViewModel.StaffAccountId}");
                Message message = await _StaffService.UpdateStaffAccountAsync(updateStaffViewModel.BranchId, updateStaffViewModel.StaffAccountId,
                 updateStaffViewModel.StaffName, updateStaffViewModel.StaffPassword, updateStaffViewModel.StaffRole);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Staff Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Staff Account with Id {updateStaffViewModel.StaffAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Staff Account.");
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{branchId}/accountId/{id}")]
        public async Task<ActionResult<Message>> DeleteStaffAccount([FromRoute] string branchId, [FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Staff Account with Id {id}");
                Message message = await _StaffService.DeleteStaffAccountAsync(branchId, id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Staff Not Found");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Staff Account with Id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Staff Account.");
            }
        }
    }
}
