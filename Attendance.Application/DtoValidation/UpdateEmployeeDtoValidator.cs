using Attendance.Application.Dto;
using FluentValidation;

namespace Attendance.Application.DtoValidation;

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100)
            .WithMessage("Full name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Department)
            .MaximumLength(50)
            .WithMessage("Department name cannot exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Department));

        RuleFor(x => x.JobTitle)
            .NotEmpty()
            .WithMessage("Job title is required.")
            .MaximumLength(50)
            .WithMessage("Job title cannot exceed 50 characters.");
    }
}