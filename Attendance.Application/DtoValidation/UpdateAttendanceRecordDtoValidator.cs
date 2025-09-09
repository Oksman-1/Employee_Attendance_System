using Attendance.Application.Dto;
using FluentValidation;

namespace Attendance.Application.DtoValidation;

public class UpdateAttendanceRecordDtoValidator : AbstractValidator<UpdateAttendanceRecordDto>
{
  public UpdateAttendanceRecordDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Attendance record Id must be greater than zero.");

        RuleFor(x => x.AttendanceDate)
            .NotEmpty()
            .WithMessage("Attendance date is required.");

        RuleFor(x => x.HoursWorked)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Hours worked cannot be negative.");

        RuleFor(x => x.ClockOutAtUtc)
            .GreaterThan(x => x.ClockInAtUtc)
            .When(x => x.ClockInAtUtc.HasValue && x.ClockOutAtUtc.HasValue)
            .WithMessage("Clock-out time must be after clock-in time.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters.");
    }
}