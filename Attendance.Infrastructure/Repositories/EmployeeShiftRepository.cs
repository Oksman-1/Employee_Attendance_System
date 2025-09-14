using Attendance.Application.Abstractions.Repositories;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Repositories;

public class EmployeeShiftRepository(ApplicationDbContext _context) : IEmployeeShiftRepository
{
    public async Task AssignEmployeeShiftAsync(int employeeId, int shiftId, DateTime assignedDate, CancellationToken ct = default)
    {
        var employeeShift = new EmployeeShift
        {
            EmployeeId = employeeId,
            ShiftId = shiftId,
            AssignedDate = assignedDate
        };
        await _context.EmployeeShifts.AddAsync(employeeShift, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UnassignEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
    {
        var assignedShift = await _context.EmployeeShifts.FindAsync([employeeShiftId], ct);//This is an overload for FindAsync that takes a cancellation token
        if (assignedShift != null)
        {
            _context.EmployeeShifts.Remove(assignedShift);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<EmployeeShift?> GetEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
    {
        return await _context.EmployeeShifts.Include(es => es.Employee)
                                            .Include(es => es.Shift)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(es => es.Id == employeeShiftId,ct);
    }

    public async Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsAsync(int employeeId, CancellationToken ct = default)
    {
       return await _context.EmployeeShifts.Include(es => es.Employee)
                                            .Include(es => es.Shift)
                                            .AsNoTracking()
                                            .Where(es => es.EmployeeId == employeeId)
                                            .ToListAsync(ct);
    }

    // Get all shifts for an employee on a specific date
    public async Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsByEmployeeIdAndDateAsync(int employeeId, DateTime date, CancellationToken ct = default)
    {
        var localDate = date.Date;
        return await _context.EmployeeShifts.Include(es => es.Shift)
                                            .AsNoTracking()
                                            .Where(es => es.EmployeeId == employeeId && es.AssignedDate.Date == localDate)
                                            .ToListAsync(ct);
    }

    // Get all employees assigned to any shift on a specific date
    public async Task<IEnumerable<EmployeeShift>> GetEmployeeShifsByDateAsync(DateTime date, CancellationToken ct = default)
    {
        var localDate = date.Date;
        return await _context.EmployeeShifts .Include(es => es.Employee)
                                             .Include(es => es.Shift)
                                             .AsNoTracking()
                                             .Where(es => es.AssignedDate.Date == localDate)
                                             .ToListAsync(ct);
    }

    public async Task<bool> IsEmployeeOnShiftAsync(int employeeId, DateTime date, CancellationToken ct = default)
    {
        var localDate = date.Date;
        return await _context.EmployeeShifts
                             .AsNoTracking()
                             .AnyAsync(es => es.EmployeeId == employeeId && es.AssignedDate.Date == localDate, ct);
    }

    public async Task DeleteEmployeeShiftAsync(int employeeShiftId, CancellationToken ct = default)
    {
        var employeeShift = await _context.EmployeeShifts.FindAsync([employeeShiftId], ct);
        if (employeeShift != null)
        {
            _context.EmployeeShifts.Remove(employeeShift);
            await _context.SaveChangesAsync(ct);
        }
    }
}