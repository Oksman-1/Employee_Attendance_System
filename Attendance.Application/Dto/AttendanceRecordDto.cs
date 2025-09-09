namespace Attendance.Application.Dto;

public record AttendanceRecordDto(
    long Id,
    int EmployeeId,
    string EmployeeName,
    DateOnly AttendanceDate,
    DateTimeOffset? ClockInAtUtc,
    DateTimeOffset? ClockOutAtUtc,
    decimal HoursWorked,
    double CalculatedHoursWorked,
    bool IsLate,
    string? Notes  
);