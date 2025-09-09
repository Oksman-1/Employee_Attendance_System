using Attendance.Application.Abstractions;
using Attendance.Application.Abstractions.Repositories;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Repositories;

public class ShiftRepository(ApplicationDbContext _context) : IShiftRepository
{
    public async Task<Shift> CreateShiftAsync(Shift shift)
    {
        await _context.Shifts.AddAsync(shift);
        await _context.SaveChangesAsync();
        return shift;
    }

    public async Task<Shift?> GetShiftByIdAsync(int shiftId)
    {
        return await _context.Shifts
                             .AsNoTracking() 
                             .FirstOrDefaultAsync(s => s.Id == shiftId);
    }

    public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
    {
        return await _context.Shifts
                             .AsNoTracking() 
                             .ToListAsync();
    }

    public async Task<Shift?> GetShiftByNameAsync(string shiftName)
    {
        return await _context.Shifts
                             .AsNoTracking() 
                             .FirstOrDefaultAsync(s => s.Name == shiftName);
    }

    public async Task UpdateShiftAsync(Shift shift)
    {
        _context.Shifts.Update(shift);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteShiftAsync(int shiftId)
    {
        var shift = await _context.Shifts.FindAsync(shiftId);
        if (shift != null)
        {
            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ShiftExistsAsync(int shiftId)
    {
        return await _context.Shifts.AnyAsync(s => s.Id == shiftId);
    }

    public async Task<bool> IsTimeWithinShiftAsync(int shiftId, TimeSpan time)
    {
        var shift = await _context.Shifts
                                  .AsNoTracking() 
                                  .FirstOrDefaultAsync(s => s.Id == shiftId);
        if (shift == null)
            return false;

        // Handle shifts that might span midnight
        if (shift.StartTime < shift.EndTime)
        {
            return time >= shift.StartTime && time <= shift.EndTime;
        }
        return time >= shift.StartTime || time <= shift.EndTime;
    }
}