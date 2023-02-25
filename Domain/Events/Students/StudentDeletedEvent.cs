namespace StudentRegistration.Domain.Events.Students;

public class StudentDeletedEvent : BaseEvent
{
    public StudentDeletedEvent(Student student)
    {
        this.Student = student;
    }

    public Student Student { get; }
}
