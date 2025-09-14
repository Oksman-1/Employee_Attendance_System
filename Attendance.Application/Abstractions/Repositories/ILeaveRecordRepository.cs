using Attendance.Domain.Entities;

namespace Attendance.Application.Abstractions.Repositories;

public interface ILeaveRecordRepository
{
    
    Task<LeaveRecord> CreateLeaveRecordAsync(LeaveRecord leaveRecord, CancellationToken ct = default);
    Task UpdateLeaveRecordAsync(LeaveRecord leaveRecord, CancellationToken ct = default);
    Task DeleteLeaveRecordAsync(int leaveRecordId, CancellationToken ct = default);
    Task<LeaveRecord?> GetLeaveRecordByIdAsync(int leaveRecordId, CancellationToken ct = default);
    Task<IEnumerable<LeaveRecord>> GetLeaveRecordsByEmployeeIdAsync(int employeeId, CancellationToken ct = default);
    Task<IEnumerable<LeaveRecord>> GetLeaveRecordsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);
    Task<bool> HasOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate, CancellationToken ct = default);
    Task<IEnumerable<LeaveRecord>> GetPendingApprovalLeavesAsync(CancellationToken ct = default);
    Task ApproveLeaveAsync(int leaveRecordId, bool approved, CancellationToken ct = default);
}
