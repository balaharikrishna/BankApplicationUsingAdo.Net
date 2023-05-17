using API.Models;
using API.ViewModels.Currency;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly IMapper _mapper;
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ILogger<CurrencyController> logger, IMapper mapper, ICurrencyService currencyService)
        {
            _logger = logger;
            _mapper = mapper;
            _currencyService = currencyService;
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("bankId/{id}")]
        public async Task<ActionResult<List<CurrencyDto>>> GetAllCurrencies([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Currencies");
                IEnumerable<Currency> currencies = await _currencyService.GetAllCurrenciesAsync(id);
                if (currencies is null || !currencies.Any())
                {
                    return NotFound("currencies Not Found.");
                }
                List<CurrencyDto> currencyDtos = _mapper.Map<List<CurrencyDto>>(currencies);
                return Ok(currencyDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Currencies Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Currencies.");
            }
        }

        [Authorize(Policy = "BranchMembersOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{bankId}/currencyCode/{code}")]
        public async Task<ActionResult<CurrencyDto>> GetCurrencyByCode([FromRoute] string bankId,[FromRoute] string code)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Currerncy with Code {code}");
                Currency currency = await _currencyService.GetCurrencyByCode(code, bankId);
                if (currency is null)
                {
                    return NotFound("currency Not Found.");
                }
                CurrencyDto currencyDto = _mapper.Map<CurrencyDto>(currency);
                return Ok(currencyDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Currency with Code {code} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Currency by Code.");
            }
        }

        [Authorize(Policy = "HeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Message>> AddCurrency([FromBody] CurrencyViewModel currencyViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Adding a new Currency");
                Message message = await _currencyService.AddCurrencyAsync(currencyViewModel.BankId, currencyViewModel.CurrencyCode, currencyViewModel.ExchangeRate);
                if (message.Result)
                {
                    return Created($"{Request.Path}/currencyCode/{message.Data}", message);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Adding a new Currency Failed");
                    return BadRequest($"An error occurred while creating Currency.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Adding a new Currency Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Currency.");
            }
        }

        [Authorize(Policy = "HeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<Message>> UpdateCurrency([FromBody] CurrencyViewModel currencyViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Updating Currency with Code {currencyViewModel.CurrencyCode}");
                Message message = await _currencyService.UpdateCurrencyAsync(currencyViewModel.BankId, currencyViewModel.CurrencyCode, currencyViewModel.ExchangeRate);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("currency Not Found.");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Currency with Code {currencyViewModel.CurrencyCode} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Currency.");
            }
        }

        [Authorize(Policy = "HeadManagerOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{bankId}/currencyCode/{code}")]
        public async Task<ActionResult<Message>> DeleteCurrency([FromRoute] string bankId, [FromRoute] string code)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Currency with Code {code}");
                Message message = await _currencyService.DeleteCurrencyAsync(bankId, code);
                if (message.Result)
                {
                    return Ok(message.ResultMessage);
                }
                else
                {
                    return NotFound("currency Not Found.");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Currency with Code {code} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Currency.");
            }
        }
    }
}
