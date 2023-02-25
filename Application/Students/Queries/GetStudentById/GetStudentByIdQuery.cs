using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Students.Queries.GetStudentById;

public record GetStudentByIdQuery(int Id) : IRequest<StudentDto>;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetStudentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

	/// <summary>
	/// Handles the <c>GetStudentByIdQuery</c>.
	/// </summary>
	/// <returns>
	/// A <c>StudentDto</c> representing the student with the requested id.
	/// </returns>
	public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
		var student = await this.context.Students.FindAsync(new object[] { request.Id }, cancellationToken);

		if (student is null)
		{
			throw new NotFoundException(nameof(Student), request.Id);
		}
	
		var teacher = await this.context.Teachers
			.Where(t => t.Id == student.AdviserIDNumber)
			.SingleAsync(cancellationToken);

		var studentDto = this.mapper.Map<StudentDto>(student);
		studentDto.Adviser = $"{teacher.FirstName} {teacher.LastName}";

		return studentDto;
	}
}
