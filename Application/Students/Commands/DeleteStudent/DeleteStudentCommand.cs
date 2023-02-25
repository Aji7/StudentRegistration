using MediatR;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Events.Students;

namespace StudentRegistration.Application.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(int Id) : IRequest;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteStudentCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

	/// <summary>
	/// Handles the <c>DeleteStudentCommand</c>.
	/// </summary>
	/// <exception cref="NotFoundException">
	/// Thrown when the student that matches request Id does not exist.
	/// </exception>
	public async Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.context.Students
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(Student), request.Id);
        }

        this.context.Students.Remove(entity);

        entity.AddDomainEvent(new StudentDeletedEvent(entity));

        await this.context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
