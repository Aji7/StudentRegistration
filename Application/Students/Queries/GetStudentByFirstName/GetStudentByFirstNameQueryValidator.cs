using FluentValidation;

namespace StudentRegistration.Application.Students.Queries.GetStudentByFirstName;

public class GetStudentByFirstNameQueryValidator : AbstractValidator<GetStudentByFirstNameQuery>
{
    public GetStudentByFirstNameQueryValidator()
    {
        RuleFor(q => q.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required.");
    }
}
