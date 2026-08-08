namespace Attendance.Application.Dto;

public record AttendanceRecordDto(
    long Id,
    int EmployeeId,
    string EmployeeName,
    DateOnly AttendanceDate,
    DateTimeOffset? ClockInAtUtc,
    DateTimeOffset? ClockOutAtUtc,
    double HoursWorked,
    bool IsLate,
    string? Notes  
);