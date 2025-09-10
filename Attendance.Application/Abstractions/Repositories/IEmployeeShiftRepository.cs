using Attendance.Domain.Entities;

namespace Attendance.Application.Abstractions.Repositories;

public interface IEmployeeShiftRepository
{
    Task AssignEmployeeShiftAsync(int employeeId, int shiftId, DateTime assignedDate, CancellationToken ct = default);
    Task UnassignEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default);
    Task<EmployeeShift?> GetEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default);
    Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsAsync(int employeeId, CancellationToken ct = default);
    Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsByEmployeeIdAndDateAsync(int employeeId, DateTime date, CancellationToken ct = default);
    Task<IEnumerable<EmployeeShift>> GetEmployeeShifsByDateAsync(DateTime date, CancellationToken ct = default);
    Task<bool> IsEmployeeOnShiftAsync(int employeeId, DateTime date, CancellationToken ct = default);
    Task DeleteEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default);
}

