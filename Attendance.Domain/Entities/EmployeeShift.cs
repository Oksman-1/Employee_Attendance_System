namespace Attendance.Domain.Entities;

public class EmployeeShift
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;
    public int ShiftId { get; set; }
    public Shift Shift { get; set; } = default!; 
    public DateTime AssignedDate { get; set; }
    
}