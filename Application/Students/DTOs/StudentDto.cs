using StudentRegistration.Application.Teachers.DTOs;

namespace StudentRegistration.Application.Students.DTOs;

public class StudentDto
{
	public int StudentIDNumber { get; init; }

	public string FirstName { get; init; }

	public string LastName { get; init; }

	public DateTime Birthday { get; init; }

	public byte Age => (byte)((DateTime.Now - Birthday).TotalDays / 365);

	public string Adviser { get; set; }

	public float OldGPA { get; init; }

	public bool IsStarSection => this.OldGPA > 95;
}
