using Attendance.Application.Abstractions;
using Attendance.Application.Abstractions.Repositories;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Repositories;

public class EmployeeRepository(ApplicationDbContext _context) : IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<Employee?> GetByCodeAsync(string qrCode, CancellationToken ct = default)
    {
       return await _context.Employees
           .AsNoTracking()
           .FirstOrDefaultAsync(e => e.QrCode == qrCode, ct);
    }

    public async Task<List<Employee>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Employees
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Employee employee, CancellationToken ct = default)
    {
        await _context.Employees.AddAsync(employee, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Employee employee, CancellationToken ct = default)
    { 
        _context.Employees.Update(employee); // No await here
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Employee employee, CancellationToken ct = default)
    {
        _context.Employees.Remove(employee); // No await here
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Employees.AnyAsync(e => e.Email == email, ct);
    }
}