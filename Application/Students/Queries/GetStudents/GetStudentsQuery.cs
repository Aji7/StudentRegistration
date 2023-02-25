using MediatR;
using Microsoft.EntityFrameworkCore;

using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Students.DTOs;

namespace StudentRegistration.Application.Students.Queries.GetStudents;

public record GetStudentsQuery : IRequest<IEnumerable<StudentDto>>;

public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, IEnumerable<StudentDto>>
{
    private readonly IApplicationDbContext context;

    public GetStudentsQueryHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

	/// <summary>
	/// Handles the <c>GetStudentsQuery</c>.
	/// </summary>
	/// <returns>
	/// All students from the data store.
	/// </returns>
	public async Task<IEnumerable<StudentDto>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
		var students = await this.context.Students.ToListAsync(cancellationToken);
		var teachers = await this.context.Teachers.ToListAsync(cancellationToken);

		return students
			.Join(teachers,
					student => student.AdviserIDNumber,
					teacher => teacher.Id,
					(student, teacher) => new StudentDto
					{
						StudentIDNumber = student.Id,
						FirstName = student.FirstName,
						LastName = student.LastName,
						Birthday = student.Birthday,
						Adviser = $"{teacher.FirstName} {teacher.LastName}",
						OldGPA = student.OldGPA
					});
	}
}
