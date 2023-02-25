using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace StudentRegistration.Infrastructure.Data;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> logger;
    private readonly ApplicationDbContext context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (this.context.Database.IsSqlServer())
            {
                await this.context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
			this.logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
}
