namespace Attendance.Application.Dto;

public class BatchCreateEmployeeResponseDto
{
    public int TotalRows { get; set; }
    public int SuccessfulCount { get; set; }
    public int FailedCount => TotalRows - SuccessfulCount;
    public List<string> Errors { get; set; } = new List<string>();
}
