using Attendance.Domain.Entities;

namespace Attendance.Application.Abstractions.Repositories;

public interface IShiftRepository
{
    Task<Shift> CreateShiftAsync(Shift shift, CancellationToken ct = default);
    Task<Shift?> GetShiftByIdAsync(int shiftId, CancellationToken ct = default);
    Task<IEnumerable<Shift>> GetAllShiftsAsync(CancellationToken ct = default);
    Task<Shift?> GetShiftByNameAsync(string shiftName, CancellationToken ct = default);
    Task UpdateShiftAsync(Shift shift, CancellationToken ct = default);
    Task DeleteShiftAsync(int shiftId, CancellationToken ct = default);
    Task<bool> ShiftExistsAsync(int shiftId, CancellationToken ct = default);
    Task<bool> IsTimeWithinShiftAsync(int shiftId, TimeSpan time, CancellationToken ct = default);
}
