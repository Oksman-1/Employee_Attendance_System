using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Domain.Entities;
using Attendance.Shared.GenericResponse;
using Microsoft.Extensions.Logging;

namespace Attendance.Application.Implementation;

public class ShiftService : IShiftService
{
    private readonly IShiftRepository _shiftRepository; 
    private readonly ILogger<ShiftService> _logger;
    
    public ShiftService(IShiftRepository shiftRepository, ILogger<ShiftService> logger)
    {
        _shiftRepository = shiftRepository ?? throw new ArgumentException(nameof(IShiftRepository));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger<ShiftService>));
    }
    
    public async Task<GenericResponse<string>> CreateShiftAsync(CreateShiftDto shift, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(CreateShiftAsync)}==============");
        
        _logger.LogInformation("Attempting to create shift with name: {ShiftName}", shift.Name);
        
        var shiftName = shift.Name.Trim();
        
        if (string.IsNullOrWhiteSpace(shift.Name))
        {
            _logger.LogWarning("Shift name is null or empty.");
            
            return GenericResponse<string>.BadRequest("Shift name cannot be empty.");
        }
        
        var existingShift = await _shiftRepository.GetShiftByNameAsync(shift.Name.Trim(), ct);
        if (existingShift != null)
        {
            _logger.LogWarning("Shift with name {ShiftName} already exists.", shift.Name);
            return GenericResponse<string>.Duplicate($"Shift with name {shiftName} already exists.");
        }
        var newShift = new Shift
        {
            Name = shift.Name,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            GracePeriodMinutes = shift.GracePeriodMinutes
        };
        
         await _shiftRepository.CreateShiftAsync(newShift, ct);
         
        _logger.LogInformation("Successfully created shift with name: {ShiftName}", shift.Name);
        
        return GenericResponse<string>.Success("Shift created successfully.", null ,"201");
    }

    public async Task<GenericResponse<ShiftDto>> GetShiftByIdAsync(int shiftId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetShiftByIdAsync)} with ShiftId: {shiftId}==============");
        
        _logger.LogInformation("Attempting to retrieve shift with id: {ShiftId}", shiftId);
        
        var shift = await _shiftRepository.GetShiftByIdAsync(shiftId, ct);
        if (shift == null)
        {
            _logger.LogWarning("Shift with ID: {ShiftId} not found", shiftId);
            return GenericResponse<ShiftDto>.NotFound("Shift not found");
        }
        
        var shiftDto = new ShiftDto
        (
            shift.Id,
            shift.Name,
            shift.StartTime,
            shift.EndTime,
            shift.GracePeriodMinutes
        );
        
        _logger.LogInformation("Successfully retrieved shift {@shift}", shift);
        
        return GenericResponse<ShiftDto>.Success("Shift retrieved successfully", shiftDto, "200");
    }

    public async Task<GenericResponse<IEnumerable<ShiftDto>>> GetAllShiftsAsync(CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetAllShiftsAsync)}==============");
        
        _logger.LogInformation($"Attempting to retrieve all shifts");
        
        var shifts = await _shiftRepository.GetAllShiftsAsync(ct);
        if (shifts == null || !shifts.Any())
        {
            _logger.LogWarning("No shifts found in the system.");
            return GenericResponse<IEnumerable<ShiftDto>>.NotFound("No shifts found");
        }
        
        var shiftDtos = shifts.Select(shift => new ShiftDto
        (
            shift.Id,
            shift.Name,
            shift.StartTime,
            shift.EndTime,
            shift.GracePeriodMinutes
        )).ToList();
        
        _logger.LogInformation("Successfully retrieved {Count} shifts.", shiftDtos.Count);
        
        return GenericResponse<IEnumerable<ShiftDto>>.Success("Shifts retrieved successfully", shiftDtos, "200");
        
    }

    public async Task<GenericResponse<ShiftDto>> GetShiftByNameAsync(string shiftName, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetShiftByNameAsync)}==============");

        _logger.LogInformation("Attempting to retrieve shift with shift name {shiftName}", shiftName);
        
        var shift = await _shiftRepository.GetShiftByNameAsync(shiftName, ct);
        if (shift == null)
        {
            _logger.LogWarning("Shift with name {ShiftName} not found", shiftName);
            return GenericResponse<ShiftDto>.NotFound("Shift not found");
        }
        
        var shiftDto = new ShiftDto
        (
            shift.Id,
            shift.Name,
            shift.StartTime,
            shift.EndTime,
            shift.GracePeriodMinutes
        );
        
        _logger.LogInformation("Successfully retrieved shift with shift name {@shiftName}", shift.Name);
        
        return GenericResponse<ShiftDto>.Success("Shift retrieved successfully", shiftDto, "200");
    }

    public async Task<GenericResponse<string>> UpdateShiftAsync(UpdateShiftDto shift, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateShiftAsync)}==============");
        
        _logger.LogInformation("Attempting to update shift with ID: {ShiftId}", shift.Id);
        
        var existingShift = await _shiftRepository.GetShiftByIdAsync(shift.Id, ct);
        if (existingShift == null)
        {
            _logger.LogWarning("Shift with ID: {ShiftId} not found", shift.Id);
            return GenericResponse<string>.NotFound("Shift not found");
        }
        
        existingShift.Name = shift.Name?.Trim() ?? existingShift.Name;
        existingShift.StartTime = shift.StartTime;
        existingShift.EndTime = shift.EndTime;
        existingShift.GracePeriodMinutes = shift.GracePeriodMinutes;    
        
        await _shiftRepository.UpdateShiftAsync(existingShift, ct);
        
        _logger.LogInformation("Successfully updated shift {@updatedShift}", existingShift);
        
        return GenericResponse<string>.Success("Shift updated successfully", null, "200");
    }

    public async Task<GenericResponse<string>> DeleteShiftAsync(int shiftId, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(DeleteShiftAsync)}==============");
        
        _logger.LogInformation("Attempting to delete shift with ID: {ShiftId}", shiftId);
        
        var existingShift = await _shiftRepository.GetShiftByIdAsync(shiftId, ct);
        if (existingShift == null)
        {
            _logger.LogWarning("Shift with ID: {ShiftId} not found", shiftId);
            return GenericResponse<string>.NotFound("Shift not found");
        }
        
        await _shiftRepository.DeleteShiftAsync(shiftId, ct);
        
        _logger.LogInformation("Successfully deleted shift with ID: {ShiftId}", shiftId);
        
        return GenericResponse<string>.Success("Shift deleted successfully", $"Shift with ID {shiftId} deleted.", "200");
    }

    public async Task<GenericResponse<bool>> IsTimeWithinShiftAsync(int shiftId, TimeSpan time, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(IsTimeWithinShiftAsync)}==============");
        
        _logger.LogInformation("Checking if time {Time} is within shift ID: {ShiftId}", time, shiftId);
        
        var shiftExists = await _shiftRepository.ShiftExistsAsync(shiftId, ct);
        if (!shiftExists)
        {
            _logger.LogWarning("Shift with ID: {ShiftId} not found", shiftId);
            return GenericResponse<bool>.NotFound("Shift not found");
        }
        
        var isWithinShift = await _shiftRepository.IsTimeWithinShiftAsync(shiftId, time, ct);
        
        _logger.LogInformation("Time {Time} is within shift ID {ShiftId}: {IsWithinShift}", time, shiftId, isWithinShift);
        
        return GenericResponse<bool>.Success("Time check completed", isWithinShift, "200");
        
    }
}