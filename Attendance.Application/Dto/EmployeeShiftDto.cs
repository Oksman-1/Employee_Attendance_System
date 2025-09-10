namespace Attendance.Application.Dto;

public record EmployeeShiftDto(
    int Id,
    int EmployeeId,
    string EmployeeName,
    int ShiftId,
    string ShiftName,
    DateTime AssignedDate
);