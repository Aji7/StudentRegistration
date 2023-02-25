using FluentValidation;

namespace StudentRegistration.Application.Students.Commands.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
		RuleFor(s => s.FirstName)
			.MaximumLength(50)
			.NotEmpty();

		RuleFor(s => s.LastName)
			.MaximumLength(50)
			.NotEmpty();

		RuleFor(s => s.OldGPA)
			.InclusiveBetween(0, 100)
			.NotEmpty();

		RuleFor(s => s.AdviserIDNumber)
			.NotEmpty();

		RuleFor(s => s.Birthday)
			.NotEmpty();
	}
}
