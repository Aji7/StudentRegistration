using AutoMapper;
using MediatR;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Common.Mappings;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Application.Teachers.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Teachers.Queries.GetTeacherById;

public record GetTeacherByIdQuery (int Id) : IRequest<TeacherDto>;

public class GetTeacherByIdQueryHandler : IRequestHandler<GetTeacherByIdQuery, TeacherDto>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetTeacherByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

	/// <summary>
	/// Handles the <c>GetTeacherByIdQuery</c>.
	/// </summary>
	/// <returns>
	/// A <c>TeacherDto</c> representing the teacher with the requested id.
	/// </returns>
	public async Task<TeacherDto> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
    {
		var teacher = await this.context.Teachers.FindAsync(new object[] { request.Id }, cancellationToken);

		if (teacher is null)
		{
			throw new NotFoundException(nameof(Teacher), request.Id);
		}
		
		var teacherDto = this.mapper.Map<TeacherDto>(teacher);
		var handledStudents = await this.context.Students
			.Where(s => s.AdviserIDNumber == teacherDto.TeacherIDNumber)
			.ProjectToListAsync<StudentDto>(this.mapper.ConfigurationProvider);

		handledStudents.ForEach(student => student.Adviser = $"{teacher.FirstName} {teacher.LastName}");
		teacherDto.HandledStudents = handledStudents;
		return teacherDto;
	}
}
