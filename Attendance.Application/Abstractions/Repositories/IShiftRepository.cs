using Attendance.Domain.Entities;

namespace Attendance.Application.Abstractions.Repositories;

public interface IShiftRepository
{
    Task<Shift> CreateShiftAsync(Shift shift);
    Task<Shift?> GetShiftByIdAsync(int shiftId);
    Task<IEnumerable<Shift>> GetAllShiftsAsync();
    Task<Shift?> GetShiftByNameAsync(string shiftName);
    Task UpdateShiftAsync(Shift shift);
    Task DeleteShiftAsync(int shiftId);
    Task<bool> ShiftExistsAsync(int shiftId);
    Task<bool> IsTimeWithinShiftAsync(int shiftId, TimeSpan time);
}