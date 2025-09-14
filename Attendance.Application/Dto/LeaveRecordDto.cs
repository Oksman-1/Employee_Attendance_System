namespace Attendance.Application.Dto;

public record LeaveRecordDto(
    int Id,
    int EmployeeId,
    string EmployeeName,
    DateTime StartDate,
    DateTime EndDate,
    string Reason,
    bool Approved
);