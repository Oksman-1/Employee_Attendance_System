using Attendance.Domain.Entities;

namespace Attendance.Application.Abstractions.Repositories;

public interface IEmployeeShiftRepository
{
    Task AssignShiftAsync(int employeeId, int shiftId, DateTime assignedDate);
    Task UnassignShiftAsync(int employeeShiftId);
    Task<EmployeeShift?> GetEmployeeShiftAsync(int employeeShiftId);
    Task<IEnumerable<EmployeeShift>> GetShiftsForEmployeeAsync(int employeeId);
    Task<IEnumerable<EmployeeShift>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime date);
    Task<IEnumerable<EmployeeShift>> GetByDateAsync(DateTime date);
    Task<bool> IsEmployeeOnShiftAsync(int employeeId, DateTime date);
    Task DeleteEmployeeShiftAsync(int employeeShiftId);
}

