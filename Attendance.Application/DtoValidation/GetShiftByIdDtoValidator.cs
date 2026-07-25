using Attendance.Application.Dto;
using FluentValidation;

namespace Attendance.Application.DtoValidation;

public class GetShiftByIdDtoValidator : AbstractValidator<GetShiftByIdDto>
{
    public GetShiftByIdDtoValidator()
    {
        RuleFor(x => x.ShiftId)
            .GreaterThan(0).WithMessage("ShiftId must be greater than 0.");
    }
}
