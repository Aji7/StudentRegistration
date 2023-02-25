using FluentValidation;

namespace StudentRegistration.Application.Teachers.Queries.GetTeacherById;

public class GetTeacherByIdQueryValidator : AbstractValidator<GetTeacherByIdQuery>
{
    public GetTeacherByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty()
            .WithMessage("Teacher ID Number is required.");
    }
}
