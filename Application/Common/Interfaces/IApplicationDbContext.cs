using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Teacher> Teachers { get; }

    DbSet<Student> Students { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
