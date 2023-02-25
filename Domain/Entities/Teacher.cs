using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Domain.Entities;

public class Teacher : Person
{
	[Required]
	public bool IsStarSectionAdviser { get; set; }
}
