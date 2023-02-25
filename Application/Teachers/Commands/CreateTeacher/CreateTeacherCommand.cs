using FluentValidation.Results;
using MediatR;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Events.Teachers;

namespace StudentRegistration.Application.Teachers.Commands.CreateTeacher;

public record CreateTeacherCommand : IRequest<int>
{
	public string FirstName { get; init; }

	public string LastName { get; init; }

	public DateTime Birthday { get; init; }

	public bool IsStarSectionAdviser { get; init; }
}

public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateTeacherCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

	/// <summary>
	/// Handles the <c>CreateTeacherCommand</c>.
	/// </summary>
	/// <returns>
	/// An int representing the newly created teacher ID.
	/// </returns>
	public async Task<int> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
		if (this.context.Teachers.Count() == 5)
		{
			var validationFailures = new List<ValidationFailure>()
			{
				new(nameof(Teacher), "Maximum capacity of 5 teachers has already been reached.")
			};

			throw new ValidationException(validationFailures);
		}
		
        var entity = new Teacher
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Birthday = request.Birthday,
            IsStarSectionAdviser = request.IsStarSectionAdviser
        };

	    entity.AddDomainEvent(new TeacherCreatedEvent(entity));

        this.context.Teachers.Add(entity);

        await this.context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
