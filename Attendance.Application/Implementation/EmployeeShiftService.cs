using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;
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

      var employeeIsOnShift = await _repository.IsEmployeeOnShiftAsync(createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.AssignedDate, ct);

        if (employeeIsOnShift)
        {
          _logger.LogWarning("Employee with ID: {EmployeeId} is already assigned to a shift on {AssignedDate}", createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.AssignedDate);

          return GenericResponse<string>.Duplicate("Employee is already assigned to a shift on the specified date", "400");
        }

      await _repository.AssignEmployeeShiftAsync(createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.ShiftId, createEmployeeShiftDto.AssignedDate, ct);

      _logger.LogInformation("Successfully assigned shift with ID: {ShiftId} to employee with ID: {EmployeeId} on {AssignedDate}", createEmployeeShiftDto.ShiftId, createEmployeeShiftDto.EmployeeId, createEmployeeShiftDto.AssignedDate);

      return GenericResponse<string>.Success("Shift assigned successfully", null, "200");

    }

    public async Task<GenericResponse<string>> UnassignEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
    {
      _logger.LogInformation($"==============Inside {nameof(UnassignEmployeeShiftAsync)}==============");

      _logger.LogInformation("Unassigning shift with EmployeeShiftId: {EmployeeShiftId}", employeeShiftId);

      var employeeShift = await _repository.GetEmployeeShiftAsync(employeeShiftId, ct);
      if (employeeShift == null)
      {
        _logger.LogWarning("EmployeeShift with ID: {EmployeeShiftId} not found", employeeShiftId);
        return GenericResponse<string>.NotFound("EmployeeShift not found");
      }

      await _repository.UnassignEmployeeShiftAsync(employeeShiftId, ct);

      _logger.LogInformation("Successfully unassigned shift with EmployeeShiftId: {EmployeeShiftId}", employeeShiftId);

      return GenericResponse<string>.Success("Shift unassigned successfully", null, "200");

    }

    public async Task<GenericResponse<EmployeeShiftDto>> GetEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
    {
      _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftAsync)}==============");

      _logger.LogInformation("Retrieving shift with EmployeeShiftId: {EmployeeShiftId}", employeeShiftId);

      var employeeShift = await _repository.GetEmployeeShiftAsync(employeeShiftId, ct);
      if (employeeShift == null)
      {
        _logger.LogWarning("EmployeeShift with ID: {EmployeeShiftId} not found", employeeShiftId);
        return GenericResponse<EmployeeShiftDto>.NotFound("EmployeeShift not found");
      }
     
      var employeeShiftDto = new EmployeeShiftDto(
            Id : employeeShift.Id,
            EmployeeId : employeeShift.EmployeeId,
            EmployeeName : employeeShift.Employee?.FullName ?? string.Empty,
            ShiftId : employeeShift.ShiftId,
            ShiftName : employeeShift.Shift?.Name ?? string.Empty,
            AssignedDate : employeeShift.AssignedDate
        );

      _logger.LogInformation("Successfully retrieved shift with EmployeeShiftId: {EmployeeShiftId}", employeeShiftId);

      return GenericResponse<EmployeeShiftDto>.Success("Success", employeeShiftDto, "200");  
    }

    public async Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsAsync(int employeeId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftsAsync)}==============");

        _logger.LogInformation("Retrieving shifts for employee with ID: {EmployeeId}", employeeId);

        var employeeShifts = await _repository.GetEmployeeShiftsAsync(employeeId, ct);

        if (!employeeShifts.Any())
        {
          _logger.LogWarning("No shifts found for employee with ID: {EmployeeId}", employeeId);

          return GenericResponse<IEnumerable<EmployeeShiftDto>>.NotFound("No shifts found for the specified employee");
        }

        var employeeShiftDtos = employeeShifts.Select(es => new EmployeeShiftDto(
              Id : es.Id,
              EmployeeId : es.EmployeeId,
              EmployeeName : es.Employee?.FullName ?? string.Empty,
              ShiftId : es.ShiftId,
              ShiftName : es.Shift?.Name ?? string.Empty,
              AssignedDate : es.AssignedDate
          )).ToList();

        _logger.LogInformation("Successfully retrieved {Count} shifts for employee with ID: {EmployeeId}", employeeShiftDtos.Count, employeeId);

        return GenericResponse<IEnumerable<EmployeeShiftDto>>.Success("Success", employeeShiftDtos, "200"); 
    }

    public async Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsByEmployeeIdAndDateAsync(int employeeId, DateTime date, CancellationToken ct = default)
    {
      _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftsByEmployeeIdAndDateAsync)}==============");

      _logger.LogInformation("Retrieving shifts for employee with ID: {EmployeeId} on date: {Date}", employeeId, date);

      var employeeShifts = await _repository.GetEmployeeShiftsByEmployeeIdAndDateAsync(employeeId, date, ct); 

      if (!employeeShifts.Any())
      {
        _logger.LogWarning("No shifts found for employee with ID: {EmployeeId} on date: {Date}", employeeId, date);

        return GenericResponse<IEnumerable<EmployeeShiftDto>>.NotFound("No shifts found for the specified employee on the given date");
      }

      var employeeShiftDtos = employeeShifts.Select(es => new EmployeeShiftDto(
            es.Id,
            es.EmployeeId,
            es.Employee?.FullName ?? string.Empty,
            es.ShiftId,
            es.Shift?.Name ?? string.Empty,
            es.AssignedDate
        )).ToList();

      _logger.LogInformation("Successfully retrieved {Count} shifts for employee with ID: {EmployeeId} on date: {Date}", employeeShiftDtos.Count, employeeId, date);

      return GenericResponse<IEnumerable<EmployeeShiftDto>>.Success("Success", employeeShiftDtos, "200");
    }

    public async Task<GenericResponse<IEnumerable<EmployeeShiftDto>>> GetEmployeeShiftsByDateAsync(DateTime date, CancellationToken ct = default)
    {
      _logger.LogInformation($"==============Inside {nameof(GetEmployeeShiftsByDateAsync)}==============");

      _logger.LogInformation("Retrieving shifts on date: {Date}", date);

      var employeeShifts = await _repository.GetEmployeeShifsByDateAsync(date, ct);

      if (!employeeShifts.Any())
      {
        _logger.LogWarning("No shifts found on date: {Date}", date);

        return GenericResponse<IEnumerable<EmployeeShiftDto>>.NotFound("No shifts found on the specified date");
      }

      var employeeShiftDtos = employeeShifts.Select(es => new EmployeeShiftDto(
            es.Id,
            es.EmployeeId,
            es.Employee?.FullName ?? string.Empty,
            es.ShiftId,
            es.Shift?.Name ?? string.Empty,
            es.AssignedDate
        )).ToList();

      _logger.LogInformation("Successfully retrieved {Count} shifts on date: {Date}", employeeShiftDtos.Count, date);

      return GenericResponse<IEnumerable<EmployeeShiftDto>>.Success("Success", employeeShiftDtos, "200");
    }

    public async Task<GenericResponse<bool>> IsEmployeeOnShiftAsync(int employeeId, DateTime date, CancellationToken ct = default)
    {
      _logger.LogInformation($"==============Inside {nameof(IsEmployeeOnShiftAsync)}==============");

      _logger.LogInformation("Checking if employee with ID: {EmployeeId} is on shift on date: {Date}", employeeId, date);
      
      var isEmployeeOnShift = await _repository.IsEmployeeOnShiftAsync(employeeId, date, ct);

      _logger.LogInformation("Employee with ID: {EmployeeId} is {OnShift}on shift on date: {Date}", employeeId, isEmployeeOnShift ? "" : "not ", date);

      return GenericResponse<bool>.Success("Success", isEmployeeOnShift, "200");  

    }

    public async Task<GenericResponse<string>> DeleteEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
    {
      _logger.LogInformation($"==============Inside {nameof(DeleteEmployeeShiftAsync)}==============");

      _logger.LogInformation("Deleting shift with EmployeeShiftId: {EmployeeShiftId}", employeeShiftId);

      if (await _repository.GetEmployeeShiftAsync(employeeShiftId, ct) is null)
      {
            _logger.LogWarning("EmployeeShift with ID: {EmployeeShiftId} not found", employeeShiftId);
            
            return GenericResponse<string>.NotFound("EmployeeShift not found");
      }

      await _repository.DeleteEmployeeShiftAsync(employeeShiftId, ct);

      _logger.LogInformation("Successfully deleted shift with EmployeeShiftId: {EmployeeShiftId}", employeeShiftId);

      return GenericResponse<string>.Success("EmployeeShift deleted successfully", null, "200");
    }
}