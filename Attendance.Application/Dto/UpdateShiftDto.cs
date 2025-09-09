namespace Attendance.Application.Dto;

public record UpdateShiftDto(
    int Id,
    string Name,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int GracePeriodMinutes
    );