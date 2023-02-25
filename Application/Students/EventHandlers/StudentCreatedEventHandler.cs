using MediatR;
using Microsoft.Extensions.Logging;
using StudentRegistration.Domain.Events.Students;

namespace StudentRegistration.Application.Students.EventHandlers;

public class StudentCreatedEventHandler : INotificationHandler<StudentCreatedEvent>
{
    private readonly ILogger<StudentCreatedEventHandler> logger;

    public StudentCreatedEventHandler(ILogger<StudentCreatedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(StudentCreatedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("StudentRegistration Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
