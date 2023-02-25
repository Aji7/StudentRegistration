using AutoMapper;
using FluentValidation.Results;
using MediatR;

using StudentRegistration.Application.Common.Exceptions;
using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Application.Common.Mappings;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Application.Teachers.DTOs;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Events.Students;

namespace StudentRegistration.Application.Students.Commands.CreateStudent;

public record CreateStudentCommand : IRequest<int>
{
	public string FirstName { get; init; }

	public string LastName { get; init; }

	public DateTime Birthday { get; init; }

	public int AdviserIDNumber { get; init; }

	public float OldGPA { get; init; }
}

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
{
    private readonly IApplicationDbContext context;
	private readonly IMapper mapper;

	public CreateStudentCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
		this.mapper = mapper;
	}

	/// <summary>
	/// Handles the <c>CreateStudentCommand</c>.
	/// </summary>
	/// <returns>
	/// An int representing the newly created student ID.
	/// </returns>
	public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
		if (this.context.Students.Count() == 25)
		{
			var validationFailures = new List<ValidationFailure>()
			{
				new(nameof(Student), "Maximum capacity of 25 students has already been reached.")
			};

			throw new ValidationException(validationFailures);
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

		var handledStudents = await this.context.Students
			.Where(s => s.AdviserIDNumber == teacher.TeacherIDNumber)
			.ProjectToListAsync<StudentDto>(this.mapper.ConfigurationProvider);

		if (handledStudents.Count >= 5)
		{
			teacher = await this.GetRandomTeacher(teachers.Where(t => t != teacher), new(), request.OldGPA > 95);
		}

		var entity = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Birthday = request.Birthday,
            AdviserIDNumber = teacher.TeacherIDNumber,
            OldGPA = request.OldGPA
        };

        entity.AddDomainEvent(new StudentCreatedEvent(entity));

        this.context.Students.Add(entity);

        await this.context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

	/// <summary>
	/// This method attempts to get a random teacher with applicable star section status and a slot for a new student.
	/// </summary>
	/// <param name="teachers">list of teachers to randomize from.</param>
	/// <param name="visitedTeachers">list of IDs of teachers visited.</param>
	/// <param name="studentIsStarSection">flag to determine if the student can be assigned to a star section adviser.</param>
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
