using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Attendance.Application.Implementation;

public class EmployeeShiftService : IEmployeeShiftService
{
     private readonly IEmployeeShiftRepository _repository;
    private readonly ILogger<EmployeeShiftService> _logger;

    public EmployeeShiftService(IEmployeeShiftRepository repository, ILogger<EmployeeShiftService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

  public async Task<GenericResponse<string>> AssignEmployeeShiftAsync(CreateEmployeeShiftDto createEmployeeShiftDto, CancellationToken ct = default)
  {
      _logger.LogInformation($"==============Inside {nameof(AssignEmployeeShiftAsync)}==============");

      _logger.LogInformation("Assigning shift to employee with ID: {EmployeeId}", createEmployeeShiftDto.EmployeeId);

      var employeeIsOnShift = await _repository.IsEmployeeOnShiftAsync(createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.AssignedDate);

      if (employeeIsOnShift)
      {
          _logger.LogWarning("Employee with ID: {EmployeeId} is already assigned to a shift on {AssignedDate}", createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.AssignedDate);

          return GenericResponse<string>.Duplicate("Employee is already assigned to a shift on the specified date", "400");
      }

      await _repository.AssignEmployeeShiftAsync(createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.ShiftId, createEmployeeShiftDto.AssignedDate);

      _logger.LogInformation("Successfully assigned shift with ID: {ShiftId} to employee with ID: {EmployeeId} on {AssignedDate}", createEmployeeShiftDto.ShiftId, createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.AssignedDate);

      return GenericResponse<string>.Success("Shift assigned successfully", null, "200");  
     
  }

  public async Task<GenericResponse<string>> UnassignEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<GenericResponse<EmployeeShiftDto>> GetEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsAsync(int employeeId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsByEmployeeIdAndDateAsync(int employeeId, DateTime date, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShifsByDateAsync(DateTime date, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsEmployeeOnShiftAsync(int employeeId, DateTime date, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<string> DeleteEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}