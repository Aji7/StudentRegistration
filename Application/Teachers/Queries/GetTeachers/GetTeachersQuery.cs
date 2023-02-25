using AutoMapper;
using MediatR;

using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Common.Mappings;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Application.Teachers.DTOs;

namespace StudentRegistration.Application.Teachers.Queries.GetTeachers;

public record GetTeachersQuery : IRequest<IEnumerable<TeacherDto>>;

public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, IEnumerable<TeacherDto>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetTeachersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

	/// <summary>
	/// Handles the <c>GetTeachersQuery</c>.
	/// </summary>
	/// <returns>
	/// All teachers from the data store.
	/// </returns>
	public async Task<IEnumerable<TeacherDto>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
    {
        var teachers = await this.context.Teachers.ProjectToListAsync<TeacherDto>(this.mapper.ConfigurationProvider);

        foreach (var teacher in teachers)
        {
			var handledStudents = await this.context.Students
				.Where(s => s.AdviserIDNumber == teacher.TeacherIDNumber)
				.ProjectToListAsync<StudentDto>(this.mapper.ConfigurationProvider);

            handledStudents.ForEach(student => student.Adviser = $"{teacher.FirstName} {teacher.LastName}");
			teacher.HandledStudents = handledStudents;
		}

        return teachers;
	}
}
