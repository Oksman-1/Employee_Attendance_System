using Attendance.API.Extension;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[ApiController]
public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;
    
    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService ?? throw new ArgumentException(nameof(IEmployeeService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger<EmployeeController>));
    }
    
    [HttpGet("api/v1/get-employee-by-id")]
    [ProducesResponseType(typeof(GenericResponse<EmployeeDto>), 200)]
    public async Task<IActionResult> GetEmployeeByIdAsync([FromQuery] int id, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeByIdAsync)} controller==============");

        var response = await _employeeService.GetEmployeeByIdAsync(id, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-employee-by-qr-code")]
    [ProducesResponseType(typeof(GenericResponse<EmployeeDto>), 200)]
    public async Task<IActionResult> GetEmployeeByQrCodeAsync([FromQuery] string qrCode, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeByQrCodeAsync)} controller==============");

        var response = await _employeeService.GetEmployeeByQrCodeAsync(qrCode, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-all-employees")]
    [ProducesResponseType(typeof(GenericResponse<List<EmployeeDto>>), 200)]
    public async Task<IActionResult> GetAllEmployeesAsync(CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetAllEmployeesAsync)} controller==============");

        var response = await _employeeService.GetAllEmployeesAsync(ct);

        return ToHttpResult(response);
    }
    
    [HttpPost("api/v1/create-employee")]
    [ServiceFilter<ValidationFilterAttribute>] // ensures DTO validation runs
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeDto createEmployeeDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(CreateEmployeeAsync)} controller==============");

        var response = await _employeeService.CreateEmployeeAsync(createEmployeeDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpPut("api/v1/update-employee")]
    [ServiceFilter<ValidationFilterAttribute>] // ensures DTO validation runs
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> UpdateEmployeeAsync([FromBody] UpdateEmployeeDto updateEmployeeDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateEmployeeAsync)} controller==============");

        var response = await _employeeService.UpdateEmployeeAsync(updateEmployeeDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpDelete("api/v1/delete-employee/{id}")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] int id, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(DeleteEmployeeAsync)} controller==============");

        var response = await _employeeService.DeleteEmployeeAsync(id, ct);

        return ToHttpResult(response);
    }
    
}