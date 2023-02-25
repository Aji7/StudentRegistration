using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Domain.Entities;

public class Student : Person
{
	[Required]
	public int AdviserIDNumber { get; set; }

	[Required]
	public float OldGPA { get; set; }
}
