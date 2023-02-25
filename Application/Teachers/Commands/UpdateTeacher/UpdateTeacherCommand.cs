using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Teachers.Commands.UpdateTeacher;

public record UpdateTeacherCommand : IRequest
{
    public int Id { get; init; }

	public string FirstName { get; init; }

	public string LastName { get; init; }

	public DateTime Birthday { get; init; }

	public bool IsStarSectionAdviser { get; init; }
}

public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateTeacherCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

	/// <summary>
	/// Handles the <c>UpdateTeacherCommand</c>.
	/// </summary>
	public async Task<Unit> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.context.Teachers.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(Teacher), request.Id);
        }

        var teacherHasNonStarSectionStudents = await this.context.Students
            .AnyAsync(s => s.AdviserIDNumber == entity.Id && s.OldGPA <= 95, cancellationToken);

		if (request.IsStarSectionAdviser && teacherHasNonStarSectionStudents)
        {
			var validationFailures = new List<ValidationFailure>()
			{
				new(nameof(request.IsStarSectionAdviser), "A Star Section Adviser may only have students with GPA > 95", request.IsStarSectionAdviser)
			};

			throw new ValidationException(validationFailures);
		}

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Birthday = request.Birthday;
        entity.IsStarSectionAdviser = request.IsStarSectionAdviser;

        await this.context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
