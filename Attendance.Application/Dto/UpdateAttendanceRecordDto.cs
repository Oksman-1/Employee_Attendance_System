namespace Attendance.Application.Dto;

public record UpdateAttendanceRecordDto(
    long Id,
    DateOnly AttendanceDate,
    DateTimeOffset? ClockInAtUtc,
    DateTimeOffset? ClockOutAtUtc,
    decimal HoursWorked,
    string? Notes
    );