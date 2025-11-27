using Attendance.Application.Dto;
using FluentValidation;

namespace Attendance.Application.DtoValidation;

public class CreateShiftDtoValidator : AbstractValidator<CreateShiftDto>
{
    public CreateShiftDtoValidator()
    {
        // Validate Shift Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Shift name is required.")
            .MaximumLength(50)
            .WithMessage("Shift name cannot exceed 50 characters.");

        // Validate StartTime
        RuleFor(x => x.StartTime)
            .NotNull()
            .WithMessage("Start time is required.");

        // Validate EndTime
        RuleFor(x => x.EndTime)
            .NotNull()
            .WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be later than start time.");

        // Validate Grace Period
        RuleFor(x => x.GracePeriodMinutes)
            .InclusiveBetween(0, 60)
            .WithMessage("Grace period must be between 0 and 60 minutes.");
    }
}