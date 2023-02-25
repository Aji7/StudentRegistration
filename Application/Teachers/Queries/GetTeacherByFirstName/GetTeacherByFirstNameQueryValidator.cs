using FluentValidation;

namespace StudentRegistration.Application.Teachers.Queries.GetTeacherByFirstName;

public class GetTeacherByFirstNameQueryValidator : AbstractValidator<GetTeacherByFirstNameQuery>
{
    public GetTeacherByFirstNameQueryValidator()
    {
        RuleFor(q => q.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required.");
    }
}
