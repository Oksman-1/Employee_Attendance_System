namespace Attendance.Application.Dto;

public record UpdateEmployeeShiftDto(
    int Id,
    int EmployeeId,
    int ShiftId,
    DateTime AssignedDate
);