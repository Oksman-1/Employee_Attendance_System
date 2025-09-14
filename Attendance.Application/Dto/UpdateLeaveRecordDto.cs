namespace Attendance.Application.Dto;

public record UpdateLeaveRecordDto(
    int Id,
    DateTime StartDate,
    DateTime EndDate,
    string Reason,
    bool Approved
);