namespace Attendance.Application.Abstractions.Services;

public interface IReportingService
{
    Task<byte[]> GenerateAttendanceReportPdfAsync(DateOnly start, DateOnly end, CancellationToken ct = default);
}