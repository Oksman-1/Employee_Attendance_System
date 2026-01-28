using Attendance.API.Extension;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[ApiController]
public class EmployeeShiftController : BaseController
{
    private readonly IEmployeeShiftService _employeeShiftService;
    private readonly ILogger<EmployeeShiftController> _logger;
    
    public EmployeeShiftController(IEmployeeShiftService employeeShiftService, ILogger<EmployeeShiftController> logger)
    {
        _employeeShiftService = employeeShiftService ?? throw new ArgumentException(nameof(IEmployeeShiftService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger<EmployeeShiftController>));
    }
    
    [HttpPost("api/v1/assign-employee-shift")]
    [ServiceFilter<ValidationFilterAttribute>]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> AssignEmployeeShiftAsync([FromBody] CreateEmployeeShiftDto createEmployeeShiftDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(AssignEmployeeShiftAsync)} controller==============");

        var response = await _employeeShiftService.AssignEmployeeShiftAsync(createEmployeeShiftDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpDelete("api/v1/unassign-employee-shift/{employeeShiftId}")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> UnassignEmployeeShiftAsync([FromRoute] int employeeShiftId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UnassignEmployeeShiftAsync)} controller==============");

        var response = await _employeeShiftService.UnassignEmployeeShiftAsync(employeeShiftId, ct);

        return ToHttpResult(response);
    }

    [HttpGet("api/v1/get-employee-shift/{employeeShiftId}")]
    [ProducesResponseType(typeof(GenericResponse<EmployeeShiftDto>), 200)]
    public async Task<IActionResult> GetEmployeeShiftAsync([FromRoute] int employeeShiftId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftAsync)} controller==============");

        var response = await _employeeShiftService.GetEmployeeShiftAsync(employeeShiftId, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-employee-shifts/{employeeId}")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<EmployeeShiftDto>>), 200)]
    public async Task<IActionResult> GetEmployeeShiftsAsync([FromRoute] int employeeId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftsAsync)} controller==============");

        var response = await _employeeShiftService.GetEmployeeShiftsAsync(employeeId, ct);

        return ToHttpResult(response);
    }

    [HttpGet("api/v1/get-employee-shifts-by-id-and-date")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<EmployeeShiftDto>>), 200)]
    public async Task<IActionResult> GetEmployeeShiftsByEmployeeIdAndDateAsync([FromQuery] int employeeId, [FromQuery] DateTime date, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftsByEmployeeIdAndDateAsync)} controller==============");

        var response = await _employeeShiftService.GetEmployeeShiftsByEmployeeIdAndDateAsync(employeeId, date, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-employee-shifts-by-date")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<EmployeeShiftDto>>), 200)]
    public async Task<IActionResult> GetEmployeeShiftsByDateAsync([FromQuery] DateTime date, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftsByDateAsync)} controller==============");

        var response = await _employeeShiftService.GetEmployeeShiftsByDateAsync(date, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/is-employee-on-shift")]
    [ProducesResponseType(typeof(GenericResponse<bool>), 200)]
    public async Task<IActionResult> IsEmployeeOnShiftAsync([FromQuery] int employeeId, [FromQuery] DateTime date, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(IsEmployeeOnShiftAsync)} controller==============");

        var response = await _employeeShiftService.IsEmployeeOnShiftAsync(employeeId, date, ct);

        return ToHttpResult(response);
    }
    
    [HttpDelete("api/v1/delete-employee-shift")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> DeleteEmployeeShiftAsync([FromQuery] int employeeShiftId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(DeleteEmployeeShiftAsync)} controller==============");

        var response = await _employeeShiftService.DeleteEmployeeShiftAsync(employeeShiftId, ct);

        return ToHttpResult(response);
    }

}