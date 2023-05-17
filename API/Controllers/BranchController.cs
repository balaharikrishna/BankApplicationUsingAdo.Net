using API.Models;
using API.ViewModels.Branch;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = "HeadManagerOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IMapper _mapper;
        private readonly IBranchService _branchService;

        public BranchController(ILogger<BranchController> logger, IMapper mapper, IBranchService branchService)
        {
            _logger = logger;
            _mapper = mapper;
            _branchService = branchService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("bankId/{id}")]
        public async Task<ActionResult<List<BranchDto>>> GetAllBranches([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Branches");
                IEnumerable<Branch> branches = await _branchService.GetAllBranchesAsync(id);
                if (branches is null || !branches.Any())
                {
                    return NotFound("Branches Not Found.");
                }
                List<BranchDto> branchDtos = _mapper.Map<List<BranchDto>>(branches);
                return Ok(branchDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Branches Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the branches.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("branchId/{id}")]
        public async Task<ActionResult<BranchDto>> GetBranchById([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Branch with id {id}");
                Branch branch = await _branchService.GetBranchByIdAsync(id);
                if (branch is null)
                {
                    return NotFound("Branch Not Found.");
                }
                BranchDto branchDto = _mapper.Map<BranchDto>(branch);
                return Ok(branchDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Branch with id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("branchName/{name}")]
        public async Task<ActionResult<BranchDto>> GetBranchByName([FromRoute] string name)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Branch with Name {name}");
                Branch branch = await _branchService.GetBranchByNameAsync(name);
                if (branch is null)
                {
                    return NotFound("Branch Not Found.");
                }
                BranchDto branchDto = _mapper.Map<BranchDto>(branch);
                return Ok(branchDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Branch with Name {name} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Fetching the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> CreateBranch([FromBody] AddBranchViewModel addBranchViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Creating a new Branch");
                Message message = await _branchService.CreateBranchAsync(addBranchViewModel.BankId, addBranchViewModel.BranchName, addBranchViewModel.BranchPhoneNumber, addBranchViewModel.BranchAddress);
                if (message.Result)
                {
                    return Created($"{Request.Path}/branchId/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Creating a new Branch Failed");
                    return BadRequest($"An error occurred while creating Branch.,Reason: {message.ResultMessage}");
                }
               
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating a new Branch Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateBranch([FromBody] UpdateBranchViewModel updateBranchViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Updating Branch with Id {updateBranchViewModel.BranchId}");
                Message message = await _branchService.UpdateBranchAsync(updateBranchViewModel.BranchId, updateBranchViewModel.BranchName, updateBranchViewModel.BranchPhoneNumber, updateBranchViewModel.BranchAddress);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Branch Not Found.");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Branch with Id {updateBranchViewModel.BranchId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("branchId/{id}")]
        public async Task<ActionResult<Message>> DeleteBranch([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Branch with Id {id}");
                Message message = await _branchService.DeleteBranchAsync(id);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("Branch Not Found.");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Branch with Id {id} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Branch.");
            }
        }
    }
}
