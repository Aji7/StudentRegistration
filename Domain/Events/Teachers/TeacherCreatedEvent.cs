namespace StudentRegistration.Domain.Events.Teachers;

public class TeacherCreatedEvent : BaseEvent
{
    public TeacherCreatedEvent(Teacher Teacher)
    {
        this.Teacher = Teacher;
    }

    public Teacher Teacher { get; }
}
