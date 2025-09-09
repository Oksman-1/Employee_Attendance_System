namespace Attendance.Application.Dto;

public record EmployeeDto(
    int Id,
    string EmployeeCode,
    string FullName,
    string Email,
    string? Department,
    string JobTitle,
    DateTime HireDate,
    bool IsActive,
    DateTimeOffset CreatedAtUtc
);