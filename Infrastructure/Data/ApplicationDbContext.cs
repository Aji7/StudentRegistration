using MediatR;
using Microsoft.EntityFrameworkCore;

using StudentRegistration.Application.Common.Interfaces;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly IMediator mediator;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : base(options)
	{
		this.mediator = mediator;
	}

	public DbSet<Teacher> Teachers => Set<Teacher>();

	public DbSet<Student> Students => Set<Student>();

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await mediator.DispatchDomainEvents(this);
		return await base.SaveChangesAsync(cancellationToken);
	}
}
