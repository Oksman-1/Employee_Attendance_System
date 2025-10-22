using System.Reflection.Metadata;
using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Domain.Common;
using QuestPDF.Fluent;

namespace Attendance.Application.Implementation;

public class ReportingService(IAttendanceRepository _attendanceRepository) : IReportingService
{
    public async Task<byte[]> GenerateAttendanceReportPdfAsync(DateOnly start, DateOnly end, CancellationToken ct = default)
    {
        var attendanceRecords = await _attendanceRepository.GetByDateRangeAsync(start, end, ct);

        var attendanceReportDoc = new AttendanceReportDocument(start, end, attendanceRecords);
        
        var pdfBytes = await Task.Run(() => attendanceReportDoc.GeneratePdf(), ct);
        
        return pdfBytes;
    }
}