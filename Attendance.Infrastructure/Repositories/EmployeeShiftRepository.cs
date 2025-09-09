using Attendance.Application.Abstractions;
using Attendance.Application.Abstractions.Repositories;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Repositories;

public class EmployeeShiftRepository(ApplicationDbContext _context) : IEmployeeShiftRepository
{
    public async Task AssignShiftAsync(int employeeId, int shiftId, DateTime assignedDate)
    {
        var employeeShift = new EmployeeShift
        {
            EmployeeId = employeeId,
            ShiftId = shiftId,
            AssignedDate = assignedDate
        };
        await _context.EmployeeShifts.AddAsync(employeeShift);
        await _context.SaveChangesAsync();
    }

    public async Task UnassignShiftAsync(int employeeShiftId)
    {
        var assignedShift = await _context.EmployeeShifts.FindAsync(employeeShiftId);
        if (assignedShift != null)
        {
            _context.EmployeeShifts.Remove(assignedShift);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EmployeeShift?> GetEmployeeShiftAsync(int employeeShiftId)
    {
        return await _context.EmployeeShifts.Include(es => es.Employee)
                                            .Include(es => es.Shift)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(es => es.Id == employeeShiftId);
    }

    public async Task<IEnumerable<EmployeeShift>> GetShiftsForEmployeeAsync(int employeeId)
    {
       return await _context.EmployeeShifts.Include(es => es.Employee)
                                            .Include(es => es.Shift)
                                            .AsNoTracking()
                                            .Where(es => es.EmployeeId == employeeId)
                                            .ToListAsync();
    }

    // Get all shifts for an employee on a specific date
    public async Task<IEnumerable<EmployeeShift>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime date)
    {
        var localDate = date.Date;
        return await _context.EmployeeShifts.Include(es => es.Shift)
                                            .AsNoTracking()
                                            .Where(es => es.EmployeeId == employeeId && es.AssignedDate.Date == localDate)
                                            .ToListAsync();
    }

    // Get all employees assigned to any shift on a specific date
    public async Task<IEnumerable<EmployeeShift>> GetByDateAsync(DateTime date)
    {
        var localDate = date.Date;
        return await _context.EmployeeShifts .Include(es => es.Employee)
                                             .Include(es => es.Shift)
                                             .AsNoTracking()
                                             .Where(es => es.AssignedDate.Date == localDate)
                                             .ToListAsync();
    }

    public async Task<bool> IsEmployeeOnShiftAsync(int employeeId, DateTime date)
    {
        var localDate = date.Date;
        return await _context.EmployeeShifts
                             .AsNoTracking()
                             .AnyAsync(es => es.EmployeeId == employeeId && es.AssignedDate.Date == localDate);
    }

    public async Task DeleteEmployeeShiftAsync(int employeeShiftId)
    {
        var employeeShift = await _context.EmployeeShifts.FindAsync(employeeShiftId);
        if (employeeShift != null)
        {
            _context.EmployeeShifts.Remove(employeeShift);
            await _context.SaveChangesAsync();
        }
    }
}