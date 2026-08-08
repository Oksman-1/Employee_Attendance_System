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
            .NotEqual(x => x.StartTime)
            .WithMessage("End time cannot be exactly the same as start time.");

        // Validate Shift Duration (1 to 16 hours)
        RuleFor(x => x)
            .Must(HaveValidDuration)
            .WithMessage("Shift duration must be between 1 and 16 hours.");

        // Validate Grace Period
        RuleFor(x => x.GracePeriodMinutes)
            .InclusiveBetween(0, 60)
            .WithMessage("Grace period must be between 0 and 60 minutes.");
    }

    private bool HaveValidDuration(CreateShiftDto dto)
    {
        TimeSpan duration;
        if (dto.EndTime > dto.StartTime)
        {
            duration = dto.EndTime - dto.StartTime;
        }
        else
        {
            // Overnight shift calculation
            duration = TimeSpan.FromHours(24) - dto.StartTime + dto.EndTime;
        }

        return duration.TotalHours >= 1 && duration.TotalHours <= 16;
    }
}