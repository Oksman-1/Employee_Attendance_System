using Attendance.Application.Dto;
using FluentValidation;

namespace Attendance.Application.DtoValidation;

public class CreateAttendanceRecordDtoValidator : AbstractValidator<CreateAttendanceRecordDto>
{
  public CreateAttendanceRecordDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("EmployeeId must be a positive number.");

        RuleFor(x => x.AttendanceDate)
            .NotEmpty().WithMessage("Attendance date is required.");

        RuleFor(x => x.HoursWorked)
            .GreaterThanOrEqualTo(0).WithMessage("Hours worked cannot be negative.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");

        // If both times are provided, enforce ordering
        RuleFor(x => x)
            .Must(x => !x.ClockInAtUtc.HasValue || !x.ClockOutAtUtc.HasValue || x.ClockOutAtUtc > x.ClockInAtUtc)
            .WithMessage("Clock-out time must be after clock-in time.");
    }
}