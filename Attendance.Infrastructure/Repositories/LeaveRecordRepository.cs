using Attendance.Application.Abstractions.Repositories;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Repositories;

public class LeaveRecordRepository(ApplicationDbContext _context) : ILeaveRecordRepository
{
   
    public async Task ApproveLeaveAsync(int leaveRecordId, bool approved, CancellationToken ct = default)
    {
        var leaveRecord = await _context.LeaveRecords.FindAsync([leaveRecordId], ct);
        if(leaveRecord != null)
        {
            leaveRecord.Approved = approved;
            await _context.SaveChangesAsync(ct);
        }

    }

    public async Task<LeaveRecord> CreateLeaveRecordAsync(LeaveRecord leaveRecord, CancellationToken ct = default)
    {
        await _context.AddAsync(leaveRecord, ct);
        await _context.SaveChangesAsync(ct);
        return leaveRecord;
    }

    public async Task DeleteLeaveRecordAsync(int leaveRecordId, CancellationToken ct = default)
    {
        var leaveRecord = await _context.LeaveRecords.FindAsync([leaveRecordId], ct);
        if(leaveRecord != null)
        {
            _context.Remove(leaveRecord);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<LeaveRecord?> GetLeaveRecordByIdAsync(int leaveRecordId, CancellationToken ct = default)
    {
        return await _context.LeaveRecords
        .Include(e => e.Employee)
        .FirstOrDefaultAsync(l => l.Id == leaveRecordId, ct);
    }

    public async Task<IEnumerable<LeaveRecord>> GetLeaveRecordsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        return await _context.LeaveRecords
        .Include(e => e.Employee)
        .Where(l => l.StartDate <= endDate && l.EndDate >= startDate)
        .ToListAsync(ct);
    }

    public async Task<IEnumerable<LeaveRecord>> GetLeaveRecordsByEmployeeIdAsync(int employeeId, CancellationToken ct = default)
    {
        return await _context.LeaveRecords
        .Include(e => e.Employee)
        .Where(l => l.EmployeeId == employeeId)
        .ToListAsync(ct);
    }

    public async Task<IEnumerable<LeaveRecord>> GetPendingApprovalLeavesAsync(CancellationToken ct = default)
    {
        return await _context.LeaveRecords
        .Include(e => e.Employee)
        .Where(l => l.Approved == false)
        .ToListAsync(ct);
    }

    public async Task<bool> HasOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        //e.g l.StartDate <= endDate → Jan 10 <= Jan 20 and l.EndDate >= startDate → Jan 15 >= Jan 12 ✅
         return await _context.LeaveRecords
            .AnyAsync(l => l.EmployeeId == employeeId &&
                           l.StartDate <= endDate &&
                           l.EndDate >= startDate, ct);
    }

    public async Task UpdateLeaveRecordAsync(LeaveRecord leaveRecord, CancellationToken ct = default)
    {
        await _context.LeaveRecords.AddAsync(leaveRecord, ct);
        await _context.SaveChangesAsync(ct);
    
    }
}