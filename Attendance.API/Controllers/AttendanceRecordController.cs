using Attendance.API.Extension;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[ApiController]
public class AttendanceRecordController : BaseController
{
    private readonly IAttendanceRecordService _attendanceRecordService;
    private readonly ILogger<AttendanceRecordController> _logger;
    
    public AttendanceRecordController(IAttendanceRecordService attendanceRecordService, ILogger<AttendanceRecordController> logger)
    {
        _attendanceRecordService = attendanceRecordService ?? throw new ArgumentException(nameof(IAttendanceRecordService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger<AttendanceRecordController>));
    }
    
    [HttpPost("api/v1/create-attendance-records")]
    [ServiceFilter<ValidationFilterAttribute>]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> CreateAttendanceRecordAsync([FromBody] CreateAttendanceRecordDto createAttendanceRecordDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(CreateAttendanceRecordAsync)} controller==============");
        
        var response = await _attendanceRecordService.CreateAttendanceRecordAsync(createAttendanceRecordDto, ct);
        
        return ToHttpResult(response);
    }
    
    [HttpPut("api/v1/update-attendance-record")]
    [ServiceFilter<ValidationFilterAttribute>]
    [ProducesResponseType(typeof(GenericResponse<string>), 200)]
    public async Task<IActionResult> UpdateAttendanceRecordAsync([FromBody] UpdateAttendanceRecordDto updateAttendanceRecordDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateAttendanceRecordAsync)} controller==============");

        var response = await _attendanceRecordService.UpdateAttendanceRecordAsync(updateAttendanceRecordDto, ct);

        return ToHttpResult(response);
    }
    
    [HttpGet("api/v1/get-attendance-records-by-date")]
    [ProducesResponseType(typeof(GenericResponse<AttendanceRecordDto>), 200)]
    public async Task<IActionResult> GetEmployeeAttendanceByDateAsync([FromQuery] int employeeId, [FromQuery] DateOnly date, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeAttendanceByDateAsync)} controller==============");

        var response = await _attendanceRecordService.GetEmployeeAttendanceByDateAsync(employeeId, date, ct);

        return ToHttpResult(response);
    }

    [HttpGet("api/v1/get-attendance-records-by-date-range")]
    [ProducesResponseType(typeof(GenericResponse<IEnumerable<AttendanceRecordDto>>), 200)]
    public async Task<IActionResult> GetAttendanceRecordsByDateRangeAsync([FromQuery] DateOnly start, [FromQuery] DateOnly end, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetAttendanceRecordsByDateRangeAsync)} controller==============");

        var response = await _attendanceRecordService.GetAttendanceRecordsByDateRangeAsync(start, end, ct);

        return ToHttpResult(response);
    }

}