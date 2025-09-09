using Attendance.Domain.Entities;

namespace Attendance.Application.Abstractions.Repositories;

public interface IAttendanceRepository
{
    Task<AttendanceRecord?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<AttendanceRecord?> GetEmployeeAndDateAsync(int employeeId, DateOnly date ,CancellationToken ct = default);
    Task<List<AttendanceRecord>> GetByDateRangeAsync(DateOnly start, DateOnly end, CancellationToken ct = default);
    Task AddAsync(AttendanceRecord record, CancellationToken ct = default);
    Task UpdateAsync(AttendanceRecord record, CancellationToken ct = default);
}