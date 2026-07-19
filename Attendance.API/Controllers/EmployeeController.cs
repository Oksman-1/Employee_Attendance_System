using Attendance.API.Extension;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Application.Dto.Auth;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Attendance.API.Controllers;

[ApiController]
public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;
    
    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger, IIdentityService identityService, IConfiguration configuration)
    {
        _employeeService = employeeService ?? throw new ArgumentException(nameof(IEmployeeService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger<EmployeeController>));
        _identityService = identityService;
        _configuration = configuration;
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
    [Authorize(Roles = "Admin")]
    [ServiceFilter<ValidationFilterAttribute>] // ensures DTO validation runs
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeDto createEmployeeDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(CreateEmployeeAsync)} controller==============");

        var response = await _employeeService.CreateEmployeeAsync(createEmployeeDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpPost("api/v1/batch-create-employees")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(GenericResponse<BatchCreateEmployeeResponseDto>), 200)]
    public async Task<IActionResult> BatchCreateEmployeesAsync(IFormFile file, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(BatchCreateEmployeesAsync)} controller==============");

        if (file == null || file.Length == 0)
        {
            return ToHttpResult(GenericResponse<BatchCreateEmployeeResponseDto>.BadRequest("File is empty or not provided."));
        }

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return ToHttpResult(GenericResponse<BatchCreateEmployeeResponseDto>.BadRequest("Invalid file format. Please upload an .xlsx file."));
        }

        using var stream = file.OpenReadStream();
        var response = await _employeeService.BatchCreateEmployeesAsync(stream, ct);

        return ToHttpResult(response);
    }

    [HttpPut("api/v1/update-employee")]
    [Authorize(Roles = "Admin")]
    [ServiceFilter<ValidationFilterAttribute>] // ensures DTO validation runs
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> UpdateEmployeeAsync([FromBody] UpdateEmployeeDto updateEmployeeDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateEmployeeAsync)} controller==============");

        var response = await _employeeService.UpdateEmployeeAsync(updateEmployeeDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpDelete("api/v1/delete-employee/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] int id, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(DeleteEmployeeAsync)} controller==============");

        var response = await _employeeService.DeleteEmployeeAsync(id, ct);

        return ToHttpResult(response);
    }
    
    [HttpPost("api/v1/reset-employee-password")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> ResetEmployeePasswordAsync([FromBody] ResetPasswordRequestDto request, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(ResetEmployeePasswordAsync)} controller==============");

        var defaultPassword = _configuration["DefaultEmployeePassword"] ?? "Password123!";
        var newPassword = string.IsNullOrWhiteSpace(request.NewPassword) ? defaultPassword : request.NewPassword;

        var response = await _identityService.ResetPasswordAsync(request.Email, newPassword);

        return ToHttpResult(response);
    }
}