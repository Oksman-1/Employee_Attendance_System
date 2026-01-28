using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;

namespace Attendance.Application.Abstractions.Services;

public interface IEmployeeService
{
    Task<GenericResponse<EmployeeDto>> GetEmployeeByIdAsync(int id, CancellationToken ct = default);
    Task<GenericResponse<EmployeeDto>> GetEmployeeByQrCodeAsync(string qrCode, CancellationToken ct = default);
    Task<GenericResponse<List<EmployeeDto>>> GetAllEmployeesAsync(CancellationToken ct = default);
    Task<GenericResponse<string>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto, CancellationToken ct = default);
    Task<GenericResponse<string>> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto, CancellationToken ct = default);
    Task<GenericResponse<string>> DeleteEmployeeAsync(int id, CancellationToken ct = default);
}