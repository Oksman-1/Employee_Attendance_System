using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;

namespace Attendance.Application.Abstractions.Services;

public interface IAttendanceRecordService
{
    Task<GenericResponse<string>> CreateAttendanceRecordAsync(CreateAttendanceRecordDto createAttendanceRecordDto, CancellationToken ct = default);
    
    Task<GenericResponse<string>> UpdateAttendanceRecordAsync(UpdateAttendanceRecordDto updateAttendanceRecordDto, CancellationToken ct = default);

    Task<GenericResponse<AttendanceRecordDto>> GetEmployeeAttendanceByDateAsync(int employeeId, DateOnly date, CancellationToken ct = default);

    Task<GenericResponse<IEnumerable<AttendanceRecordDto>>> GetAttendanceRecordsByDateRangeAsync(DateOnly start, DateOnly end, CancellationToken ct = default);
}