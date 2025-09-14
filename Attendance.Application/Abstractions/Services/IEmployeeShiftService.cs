using Attendance.Application.Dto;
using Attendance.Domain.Common;

namespace Attendance.Application.Abstractions.Services;

public interface IEmployeeShiftService
{
    Task<GenericResponse<string>> AssignEmployeeShiftAsync(CreateEmployeeShiftDto createEmployeeShiftDto, CancellationToken ct = default);
    Task<GenericResponse<string>> UnassignEmployeeShiftAsync( int employeeShiftId, CancellationToken ct = default);
    Task<GenericResponse<EmployeeShiftDto>> GetEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default);
    Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsAsync(int employeeId, CancellationToken ct = default);
    Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsByEmployeeIdAndDateAsync(int employeeId, DateTime date,CancellationToken ct = default);
    Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsByDateAsync(DateTime date, CancellationToken ct = default);
    Task<GenericResponse<bool>> IsEmployeeOnShiftAsync(int employeeId, DateTime date, CancellationToken ct = default);
    Task<GenericResponse<string>> DeleteEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default);
}