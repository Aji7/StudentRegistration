using AutoMapper;
using FluentValidation.Results;
using MediatR;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Common.Mappings;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Application.Teachers.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Students.Commands.UpdateStudent;

public record UpdateStudentCommand : IRequest
{
    public int Id { get; init; }

	public string FirstName { get; init; }

	public string LastName { get; init; }

	public DateTime Birthday { get; init; }

	public int AdviserIDNumber { get; init; }

	public float OldGPA { get; init; }
}

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
{
	private readonly IApplicationDbContext context;
	private readonly IMapper mapper;

	public UpdateStudentCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		this.context = context;
		this.mapper = mapper;
	}

	/// <summary>
	/// Handles the <c>UpdateStudentCommand</c>.
	/// </summary>
	public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.context.Students
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(Student), request.Id);
        }

		var teachers = await this.context.Teachers.ProjectToListAsync<TeacherDto>(this.mapper.ConfigurationProvider);
		if (teachers.Count == 0)
		{
			var validationFailures = new List<ValidationFailure>()
			{
				new(nameof(request.AdviserIDNumber), "No Available Teacher")
			};

			throw new ValidationException(validationFailures);
		}

		var teacher = teachers.Where(t => t.TeacherIDNumber == request.AdviserIDNumber).SingleOrDefault();
		if (teacher is default(TeacherDto))
		{
			var validationFailures = new List<ValidationFailure>()
			{
				new(nameof(request.AdviserIDNumber), "Teacher Not Found", request.AdviserIDNumber)
			};

			throw new ValidationException(validationFailures);
		}

		if (teacher.IsStarSectionAdviser && request.OldGPA <= 95)
		{
			var validationFailures = new List<ValidationFailure>()
			{
				new(nameof(request.AdviserIDNumber), "Only Qualifified Students (GPA > 95) can be assigned to a Star Section Adviser", request.AdviserIDNumber)
			};

			throw new ValidationException(validationFailures);
		}

		if (request.AdviserIDNumber != entity.AdviserIDNumber)
		{
			var handledStudents = await this.context.Students
				.Where(s => s.AdviserIDNumber == teacher.TeacherIDNumber)
				.ProjectToListAsync<StudentDto>(this.mapper.ConfigurationProvider);

			if (handledStudents.Count >= 5)
			{
				teacher = await this.GetRandomTeacher(teachers.Where(t => t != teacher), new(), request.OldGPA > 95);
			}
		}

		entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Birthday = request.Birthday;
        entity.AdviserIDNumber = request.AdviserIDNumber;
        entity.OldGPA = request.OldGPA;

        await this.context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

	private async Task<TeacherDto> GetRandomTeacher(IEnumerable<TeacherDto> teachers, List<int> visitedTeachers, bool studentIsStarSection)
	{
		if (visitedTeachers.Count == teachers.Count())
		{
			var validationFailures = new List<ValidationFailure>()
			{
				new("AdviserIDNumber", "No Available Teacher")
			};

			throw new ValidationException(validationFailures);
		}

		var randomTeacher = teachers.ElementAt(new Random().Next(0, teachers.Count() - 1));

		if (visitedTeachers.Contains(randomTeacher.TeacherIDNumber))
		{
			return await this.GetRandomTeacher(teachers, visitedTeachers, studentIsStarSection);
		}

		visitedTeachers.Add(randomTeacher.TeacherIDNumber);

		if (randomTeacher.IsStarSectionAdviser && !studentIsStarSection)
		{
			return await this.GetRandomTeacher(teachers, visitedTeachers, studentIsStarSection);
		}

		var handledStudents = await this.context.Students
			.Where(s => s.AdviserIDNumber == randomTeacher.TeacherIDNumber)
			.ProjectToListAsync<StudentDto>(this.mapper.ConfigurationProvider);

		if (handledStudents.Count >= 5)
		{
			return await this.GetRandomTeacher(teachers, visitedTeachers, studentIsStarSection);
		}

		return randomTeacher;
	}

}
