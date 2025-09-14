namespace Attendance.Application.Dto;

public record CreateLeaveRecordDto(
    int EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    string Reason
);