using FluentValidation;

namespace StudentRegistration.Application.Students.Queries.GetStudentById;

public class GetStudentByIdQueryValidator : AbstractValidator<GetStudentByIdQuery>
{
    public GetStudentByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty()
            .WithMessage("Student ID Number is required.");
    }
}
