using FluentValidation;

namespace StudentRegistration.Application.Teachers.Commands.UpdateTeacher;

public class UpdateTeacherCommandValidator : AbstractValidator<UpdateTeacherCommand>
{
    public UpdateTeacherCommandValidator()
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
