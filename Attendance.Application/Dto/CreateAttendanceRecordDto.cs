namespace Attendance.Application.Dto;

public record CreateAttendanceRecordDto(
    int EmployeeId,
    DateOnly AttendanceDate,
    DateTimeOffset? ClockInAtUtc,
    DateTimeOffset? ClockOutAtUtc,
    decimal HoursWorked,
    string? Notes
    );