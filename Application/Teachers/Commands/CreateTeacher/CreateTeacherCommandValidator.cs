using FluentValidation;

namespace StudentRegistration.Application.Teachers.Commands.CreateTeacher;

public class CreateTeacherCommandValidator : AbstractValidator<CreateTeacherCommand>
{
    public CreateTeacherCommandValidator()
    {
		RuleFor(t => t.FirstName)
			.MaximumLength(50)
			.NotEmpty();

		RuleFor(t => t.LastName)
			.MaximumLength(50)
			.NotEmpty();

		RuleFor(t => t.Birthday)
			.NotEmpty();
	}
}
