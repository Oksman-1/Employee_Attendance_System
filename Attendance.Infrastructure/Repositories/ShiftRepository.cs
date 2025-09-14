using Attendance.Application.Abstractions.Repositories;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Repositories;

public class ShiftRepository(ApplicationDbContext _context) : IShiftRepository
{
    public async Task<Shift> CreateShiftAsync(Shift shift, CancellationToken ct = default)
    {
        await _context.Shifts.AddAsync(shift,ct);
        await _context.SaveChangesAsync(ct);
        return shift;
    }

    public async Task<Shift?> GetShiftByIdAsync(int shiftId, CancellationToken ct = default)
    {
        return await _context.Shifts
                             .AsNoTracking() 
                             .FirstOrDefaultAsync(s => s.Id == shiftId, ct);
    }

    public async Task<IEnumerable<Shift>> GetAllShiftsAsync(CancellationToken ct = default)
    {
        return await _context.Shifts
                             .AsNoTracking() 
                             .ToListAsync(ct);
    }

    public async Task<Shift?> GetShiftByNameAsync(string shiftName, CancellationToken ct = default)
    {
        return await _context.Shifts
                             .AsNoTracking() 
                             .FirstOrDefaultAsync(s => s.Name == shiftName, ct);
    }

    public async Task UpdateShiftAsync(Shift shift, CancellationToken ct = default)
    {
        _context.Shifts.Update(shift);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteShiftAsync(int shiftId, CancellationToken ct = default)
    {
        var shift = await _context.Shifts.FindAsync([shiftId],ct);
        if (shift != null)
        {
            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> ShiftExistsAsync(int shiftId, CancellationToken ct = default)
    {
        return await _context.Shifts.AnyAsync(s => s.Id == shiftId, ct);
    }

    public async Task<bool> IsTimeWithinShiftAsync(int shiftId, TimeSpan time, CancellationToken ct = default)
    {
        var shift = await _context.Shifts
                                  .AsNoTracking() 
                                  .FirstOrDefaultAsync(s => s.Id == shiftId, ct);
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