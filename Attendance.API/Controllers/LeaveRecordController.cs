using Attendance.API.Extension;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[ApiController]
public class LeaveRecordController : BaseController
{
    private readonly ILeaveRecordService _leaveRecordService;
    private readonly ILogger<LeaveRecordController> _logger;
    
    public LeaveRecordController(ILeaveRecordService leaveRecordService, ILogger<LeaveRecordController> logger)
    {
        _leaveRecordService = leaveRecordService ?? throw new ArgumentException(nameof(ILeaveRecordService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger<LeaveRecordController>));
    }
    
    [HttpPost("api/v1/create-leave-record")]
    [ServiceFilter<ValidationFilterAttribute>]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> CreateLeaveRecordAsync([FromBody] CreateLeaveRecordDto createLeaveRecordDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(CreateLeaveRecordAsync)} controller==============");
        
        var response = await _leaveRecordService.CreateLeaveRecordAsync(createLeaveRecordDto, ct);
        
        return ToHttpResult(response);
    }
    
    [HttpPut("api/v1/update-leave-record")]
    [ServiceFilter<ValidationFilterAttribute>]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> UpdateLeaveRecordAsync([FromBody] UpdateLeaveRecordDto updateLeaveRecordDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateLeaveRecordAsync)} controller==============");

        var response = await _leaveRecordService.UpdateLeaveRecordAsync(updateLeaveRecordDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpDelete("api/v1/delete-leave-record/{leaveRecordId}")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> DeleteLeaveRecordAsync([FromRoute] int leaveRecordId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(DeleteLeaveRecordAsync)} controller==============");

        var response = await _leaveRecordService.DeleteLeaveRecordAsync(leaveRecordId, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-leave-record/{leaveRecordId}")]
    [ProducesResponseType(typeof(GenericResponse<LeaveRecordDto>), 200)]
    public async Task<IActionResult> GetLeaveRecordByIdAsync([FromRoute] int leaveRecordId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetLeaveRecordByIdAsync)} controller==============");

        var response = await _leaveRecordService.GetLeaveRecordByIdAsync(leaveRecordId, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-leave-records-by-employee/{employeeId}")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<LeaveRecordDto>>), 200)]
    public async Task<IActionResult> GetLeaveRecordsByEmployeeIdAsync([FromRoute] int employeeId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetLeaveRecordsByEmployeeIdAsync)} controller==============");

        var response = await _leaveRecordService.GetLeaveRecordsByEmployeeIdAsync(employeeId, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-leave-records-by-date-range")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<LeaveRecordDto>>), 200)]
    public async Task<IActionResult> GetLeaveRecordsByDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetLeaveRecordsByDateRangeAsync)} controller==============");

        var response = await _leaveRecordService.GetLeaveRecordsByDateRangeAsync(startDate, endDate, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/check-overlapping-leave")]
    [ProducesResponseType(typeof(GenericResponse<bool>), 200)]
    public async Task<IActionResult> HasOverlappingLeaveAsync([FromQuery] int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(HasOverlappingLeaveAsync)} controller==============");

        var response = await _leaveRecordService.HasOverlappingLeaveAsync(employeeId, startDate, endDate, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-pending-approval-leaves")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<LeaveRecordDto>>), 200)]
    public async Task<IActionResult> GetPendingApprovalLeavesAsync(CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetPendingApprovalLeavesAsync)} controller==============");

        var response = await _leaveRecordService.GetPendingApprovalLeavesAsync(ct);

        return ToHttpResult(response);
    }
    
    [HttpPut("api/v1/approve-leave/{leaveRecordId}")]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> ApproveLeaveAsync([FromRoute] int leaveRecordId, [FromQuery] bool approved, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(ApproveLeaveAsync)} controller==============");

        var response = await _leaveRecordService.ApproveLeaveAsync(leaveRecordId, approved, ct);

        return ToHttpResult(response);
    }
}