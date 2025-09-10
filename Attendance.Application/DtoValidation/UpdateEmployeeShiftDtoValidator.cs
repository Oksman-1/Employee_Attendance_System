using Attendance.Application.Dto;
using FluentValidation;

namespace Attendance.Application.DtoValidation;

public class UpdateEmployeeShiftDtoValidator : AbstractValidator<UpdateEmployeeShiftDto>
{
   public UpdateEmployeeShiftDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("EmployeeId must be greater than 0.");

        RuleFor(x => x.ShiftId)
            .GreaterThan(0).WithMessage("ShiftId must be greater than 0.");

        RuleFor(x => x.AssignedDate)
            .NotEmpty().WithMessage("AssignedDate is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("AssignedDate cannot be in the future.");
    }
}