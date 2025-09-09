namespace Attendance.Application.Dto;

public record CreateEmployeeDto(
        string EmployeeCode,
        string FullName,
        string Email,
        string? Department,
        string JobTitle,
        DateTime HireDate
    );
    