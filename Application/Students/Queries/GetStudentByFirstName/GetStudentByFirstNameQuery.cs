using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Students.Queries.GetStudentByFirstName;

public record GetStudentByFirstNameQuery(string FirstName) : IRequest<StudentDto>;

public class GetStudentByFirstNameQueryHandler : IRequestHandler<GetStudentByFirstNameQuery, StudentDto>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetStudentByFirstNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

	/// <summary>
	/// Handles the <c>GetStudentByFirstNameQuery</c>.
	/// </summary>
	/// <returns>
	/// A <c>StudentDto</c> representing the student with the requested first name.
	/// </returns>
	public async Task<StudentDto> Handle(GetStudentByFirstNameQuery request, CancellationToken cancellationToken)
    {
        var student = await this.context.Students
            .Where(s => s.FirstName.Contains(request.FirstName))
            .SingleOrDefaultAsync(cancellationToken);

        if (student is default(Student))
        {
			throw new NotFoundException(nameof(Student), request.FirstName);
		}

        var teacher = await this.context.Teachers
            .Where(t => t.Id == student.AdviserIDNumber)
            .SingleAsync(cancellationToken);

        var studentDto = this.mapper.Map<StudentDto>(student);
        studentDto.Adviser = $"{teacher.FirstName} {teacher.LastName}";

        return studentDto;
    }
}
