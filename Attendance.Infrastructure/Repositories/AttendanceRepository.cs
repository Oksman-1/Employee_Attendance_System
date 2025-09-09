using Attendance.Application.Abstractions.Repositories;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Repositories;

public class AttendanceRepository (ApplicationDbContext _context) : IAttendanceRepository
{

     public async Task<AttendanceRecord?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.AttendanceRecords
            .Include(e => e.Employee)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }
    public async Task<AttendanceRecord?> GetEmployeeAndDateAsync(int employeeId, DateOnly date, CancellationToken ct = default)
    {
        return await _context.AttendanceRecords
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId 
                                      && a.AttendanceDate == date, ct);
    }

    public async Task<List<AttendanceRecord>> GetByDateRangeAsync(DateOnly start, DateOnly end, CancellationToken ct = default)
    {
        return await _context.AttendanceRecords
            .Include(e => e.Employee)
            .Where(a => a.AttendanceDate >= start && a.AttendanceDate <= end)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(AttendanceRecord record, CancellationToken ct = default)
    {
        await _context.AttendanceRecords.AddAsync(record, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(AttendanceRecord record, CancellationToken ct = default)
    {
        _context.AttendanceRecords.Update(record);
        await _context.SaveChangesAsync(ct);
    }

}