using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRegistration.Domain.Entities;

public class Person : BaseEntity
{
	[Required]
	[MaxLength(50)]
	[Column(Order = 1)]
	public string FirstName { get; set; }

	[Required]
	[MaxLength(50)]
	[Column(Order = 2)]
	public string LastName { get; set; }

	[Required]
	public DateTime Birthday { get; set; }
}
