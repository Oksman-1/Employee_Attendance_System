namespace Attendance.Application.Dto;

public record UpdateEmployeeDto( 
    int EmployeeId,
    string FullName,
    string Email,
    string? Department,
    string JobTitle,
    bool IsActive
    );