namespace Attendance.Application.Dto;

public record CreateShiftDto(
    string Name,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int GracePeriodMinutes = 10
);