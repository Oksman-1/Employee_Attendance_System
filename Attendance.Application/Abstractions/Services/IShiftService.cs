using Attendance.Application.Dto;
using Attendance.Domain.Common;
using Attendance.Domain.Entities;

namespace Attendance.Application.Abstractions.Services;

public interface IShiftService
{
    Task<GenericResponse<string>> CreateShiftAsync(CreateShiftDto shiftDto);
    Task<GenericResponse<ShiftDto>> GetShiftByIdAsync(int shiftId);
    Task<GenericResponse<IEnumerable<ShiftDto>>> GetAllShiftsAsync();
    Task<GenericResponse<ShiftDto>> GetShiftByNameAsync(string shiftName);
    Task<GenericResponse<string>> UpdateShiftAsync(UpdateShiftDto shiftDto);
    Task<GenericResponse<string>> DeleteShiftAsync(int shiftId);
    Task<GenericResponse<bool>> IsTimeWithinShiftAsync(int shiftId, TimeSpan time);
}
