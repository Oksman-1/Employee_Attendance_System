using Attendance.Application.Dto;
using Attendance.Shared.GenericResponse;

namespace Attendance.Application.Abstractions.Services;

public interface IShiftService
{
    Task<GenericResponse<string>> CreateShiftAsync(CreateShiftDto shiftDto, CancellationToken ct = default);
    Task<GenericResponse<ShiftDto>> GetShiftByIdAsync(int shiftId, CancellationToken ct = default);
    Task<GenericResponse<IEnumerable<ShiftDto>>> GetAllShiftsAsync(CancellationToken ct = default);
    Task<GenericResponse<ShiftDto>> GetShiftByNameAsync(string shiftName, CancellationToken ct = default);
    Task<GenericResponse<string>> UpdateShiftAsync(UpdateShiftDto shiftDto, CancellationToken ct = default);
    Task<GenericResponse<string>> DeleteShiftAsync(int shiftId, CancellationToken ct = default);
    Task<GenericResponse<bool>> IsTimeWithinShiftAsync(int shiftId, TimeSpan time, CancellationToken ct = default);
}

