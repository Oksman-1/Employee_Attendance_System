using Attendance.Application.Dto;
using Attendance.Domain.Common;

namespace Attendance.Application.Abstractions.Services;

public interface ILeaveRecordService
{
    Task<GenericResponse<string>> CreateLeaveRecordAsync(CreateLeaveRecordDto dto, CancellationToken ct = default);
    Task<GenericResponse<string>> UpdateLeaveRecordAsync(UpdateLeaveRecordDto dto, CancellationToken ct = default);
    Task<GenericResponse<string>> DeleteLeaveRecordAsync(int leaveRecordId, CancellationToken ct = default);
    Task<GenericResponse<LeaveRecordDto>> GetLeaveRecordByIdAsync(int leaveRecordId, CancellationToken ct = default);
    Task<GenericResponse<IEnumerable<LeaveRecordDto>>> GetLeaveRecordsByEmployeeIdAsync(int employeeId, CancellationToken ct = default);
    Task<GenericResponse<IEnumerable<LeaveRecordDto>>> GetLeaveRecordsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);
    Task<GenericResponse<bool>> HasOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate, CancellationToken ct = default);
    Task<GenericResponse<IEnumerable<LeaveRecordDto>>> GetPendingApprovalLeavesAsync(CancellationToken ct = default);
    Task<GenericResponse<string>> ApproveLeaveAsync(int leaveRecordId, bool approved, CancellationToken ct = default);
}