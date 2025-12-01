using Attendance.API.Extension;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[ApiController]
public class ShiftController : BaseController
{
    private readonly IShiftService _shiftService;
    private readonly ILogger<ShiftController> _logger;
    
    public ShiftController(IShiftService shiftService, ILogger<ShiftController> logger)
    {
        _shiftService = shiftService ?? throw new ArgumentException(nameof(IShiftService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger<ShiftController>));
    }
    
    [HttpPost("api/v1/create-shift")]
    [ServiceFilter<ValidationFilterAttribute>]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> CreateShiftAsync([FromBody] CreateShiftDto createShiftDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(CreateShiftAsync)} controller==============");
        
        var response = await _shiftService.CreateShiftAsync(createShiftDto, ct);
        
        return ToHttpResult(response);
    }
    
    [HttpPut("api/v1/update-shift")]
    [ServiceFilter<ValidationFilterAttribute>]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> UpdateShiftAsync([FromBody] UpdateShiftDto updateShiftDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateShiftAsync)} controller==============");

        var response = await _shiftService.UpdateShiftAsync(updateShiftDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-shift-by-id")]
    [ProducesResponseType(typeof(GenericResponse<ShiftDto>), 200)]
    public async Task<IActionResult> GetShiftByIdAsync([FromQuery] int shiftId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetShiftByIdAsync)} controller==============");

        var response = await _shiftService.GetShiftByIdAsync(shiftId, ct);

        return ToHttpResult(response);
    }

    [HttpGet("api/v1/get-all-shifts")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<ShiftDto>>), 200)]
    public async Task<IActionResult> GetAllShiftsAsync(CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetAllShiftsAsync)} controller==============");

        var response = await _shiftService.GetAllShiftsAsync(ct);

        return ToHttpResult(response);
    }

    [HttpGet("api/v1/get-shift-by-name")]
    [ProducesResponseType(typeof(GenericResponse<ShiftDto>), 200)]
    public async Task<IActionResult> GetShiftByNameAsync([FromQuery] string shiftName, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetShiftByNameAsync)} controller==============");

        var response = await _shiftService.GetShiftByNameAsync(shiftName, ct);

        return ToHttpResult(response);
    }

    [HttpDelete("api/v1/delete-shift")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> DeleteShiftAsync([FromQuery] int shiftId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(DeleteShiftAsync)} controller==============");

        var response = await _shiftService.DeleteShiftAsync(shiftId, ct);

        return ToHttpResult(response);
    }

    [HttpGet("api/v1/is-time-within-shift")]
    [ProducesResponseType(typeof(GenericResponse<bool>), 200)]
    public async Task<IActionResult> IsTimeWithinShiftAsync([FromQuery] int shiftId, [FromQuery] TimeSpan time, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(IsTimeWithinShiftAsync)} controller==============");

        var response = await _shiftService.IsTimeWithinShiftAsync(shiftId, time, ct);

        return ToHttpResult(response);
    }
}