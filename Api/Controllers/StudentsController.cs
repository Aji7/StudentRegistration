using Microsoft.AspNetCore.Mvc;

using StudentRegistration.Api.Controllers;
using StudentRegistration.Application.Students.Commands.CreateStudent;
using StudentRegistration.Application.Students.Commands.DeleteStudent;
using StudentRegistration.Application.Students.Commands.UpdateStudent;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Application.Students.Queries.GetStudentByFirstName;
using StudentRegistration.Application.Students.Queries.GetStudentById;
using StudentRegistration.Application.Students.Queries.GetStudents;

namespace SchoolRegistration.Api.Controllers;

[Route("api/students")]
[ApiController]
public class StudentsController : ApiControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<StudentDto>> GetStudentsAsync()
	{
		return await this.Mediator.Send(new GetStudentsQuery());
	}

	[HttpGet]
	[Route("{id:int}")]
	public async Task<ActionResult<StudentDto>> GetStudentByIdAsync(int id)
	{
		return await this.Mediator.Send(new GetStudentByIdQuery(id));
	}

	[HttpGet]
	[Route("{firstName}")]
	public async Task<ActionResult<StudentDto>> GetStudentByFirstNameAsync(string firstName)
	{
		return await this.Mediator.Send(new GetStudentByFirstNameQuery(firstName));
	}

	[HttpPost]
	public async Task<ActionResult<int>> CreateStudentAsync(CreateStudentCommand command)
	{
		return await this.Mediator.Send(command);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesDefaultResponseType]
	public async Task<IActionResult> UpdateStudentAsync(UpdateStudentCommand command)
	{
		await this.Mediator.Send(command);
		return NoContent();
	}

	[HttpDelete]
	[Route("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesDefaultResponseType]
	public async Task<IActionResult> DeleteStudentAsync(int id)
	{
		await this.Mediator.Send(new DeleteStudentCommand(id));
		return NoContent();
	}
}
