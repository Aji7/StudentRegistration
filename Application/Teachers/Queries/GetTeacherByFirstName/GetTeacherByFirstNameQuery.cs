using AutoMapper;

using MediatR;
using Microsoft.EntityFrameworkCore;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Common.Mappings;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Application.Teachers.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Teachers.Queries.GetTeacherByFirstName;

public record GetTeacherByFirstNameQuery(string FirstName) : IRequest<TeacherDto>;

public class GetTeacherByFirstNameQueryHandler : IRequestHandler<GetTeacherByFirstNameQuery, TeacherDto>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetTeacherByFirstNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

	/// <summary>
	/// Handles the <c>GetTeacherByNameQuery</c>.
	/// </summary>
	/// <returns>
	/// A <c>TeacherDto</c> representing the teacher with the requested first name.
	/// </returns>
	public async Task<TeacherDto> Handle(GetTeacherByFirstNameQuery request, CancellationToken cancellationToken)
    {
		var teacher = await this.context.Teachers
			.Where(s => s.FirstName.Contains(request.FirstName))
			.SingleOrDefaultAsync(cancellationToken);

		if (teacher is default(Teacher))
		{
			throw new NotFoundException(nameof(Teacher), request.FirstName);
		}

		var teacherDto = this.mapper.Map<TeacherDto>(teacher);
		var handledStudents = await this.context.Students
			.Where(s => s.AdviserIDNumber == teacherDto.TeacherIDNumber)
			.ProjectToListAsync<StudentDto>(this.mapper.ConfigurationProvider);

		handledStudents.ForEach(student => student.Adviser = $"{teacherDto.FirstName} {teacherDto.LastName}");
		teacherDto.HandledStudents = handledStudents;
		return teacherDto;
	}
}
