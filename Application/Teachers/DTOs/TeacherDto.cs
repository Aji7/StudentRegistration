using StudentRegistration.Application.Students.DTOs;

namespace StudentRegistration.Application.Teachers.DTOs;

public class TeacherDto
{
	public int TeacherIDNumber { get; init; }

	public string FirstName { get; init; }

	public string LastName { get; init; }

	public DateTime Birthday { get; init; }

	public byte Age => (byte)((DateTime.Now - this.Birthday).TotalDays / 365);

	public IEnumerable<StudentDto> HandledStudents { get; set; } = Enumerable.Empty<StudentDto>();

	public bool IsStarSectionAdviser { get; init; }
}
