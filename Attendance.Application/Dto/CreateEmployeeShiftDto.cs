namespace Attendance.Application.Dto;

public record CreateEmployeeShiftDto(
    int EmployeeId,
    int ShiftId,
    DateTime AssignedDate
);