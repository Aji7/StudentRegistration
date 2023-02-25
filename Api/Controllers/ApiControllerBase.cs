using MediatR;
using Microsoft.AspNetCore.Mvc;

using StudentRegistration.Api.Filters;

namespace StudentRegistration.Api.Controllers;

[ApiController]
[ApiExceptionFilter]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? mediator;

    protected ISender Mediator => this.mediator ??= this.HttpContext.RequestServices.GetRequiredService<ISender>();
}
