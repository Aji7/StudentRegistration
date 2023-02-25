using Microsoft.AspNetCore.Mvc;

using StudentRegistration.Api.Controllers;
using StudentRegistration.Application.Teachers.Commands.CreateTeacher;
using StudentRegistration.Application.Teachers.Commands.UpdateTeacher;
using StudentRegistration.Application.Teachers.DTOs;
using StudentRegistration.Application.Teachers.Queries.GetTeacherByFirstName;
using StudentRegistration.Application.Teachers.Queries.GetTeacherById;
using StudentRegistration.Application.Teachers.Queries.GetTeachers;

namespace SchoolRegistration.Api.Controllers;

[Route("api/teachers")]
[ApiController]
public class TeachersController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TeacherDto>> GetTeachersAsync()
    {
        return await this.Mediator.Send(new GetTeachersQuery());
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<TeacherDto>> GetTeacherByIdAsync(int id)
    {
        return await this.Mediator.Send(new GetTeacherByIdQuery(id));
    }

    [HttpGet]
    [Route("{firstName}")]
    public async Task<ActionResult<TeacherDto>> GetTeacherByFirstNameAsync(string firstName)
    {
        return await this.Mediator.Send(new GetTeacherByFirstNameQuery(firstName));
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTeacherAsync(CreateTeacherCommand command)
    {
        return await this.Mediator.Send(command);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateTeacherAsync(UpdateTeacherCommand command)
    {
        await this.Mediator.Send(command);
        return NoContent();
    }
}
