namespace StudentRegistration.Domain.Events.Students;

public class StudentCreatedEvent : BaseEvent
{
    public StudentCreatedEvent(Student student)
    {
        this.Student = student;
    }

    public Student Student { get; }
}
