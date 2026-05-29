using System;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EducationPlatform.Extensions;

internal static class DatabaseMigrationExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        const int maxAttempts = 10;

        await using var scope = app.Services.CreateAsyncScope();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigration");
        var dbContext = scope.ServiceProvider.GetRequiredService<EducationDbContext>();

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                logger.LogInformation("Applying database migrations. Attempt {Attempt}/{MaxAttempts}.", attempt, maxAttempts);
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied.");
                return;
            }
            catch (Exception exception) when (attempt < maxAttempts)
            {
                logger.LogWarning(
                    exception,
                    "Database migration failed. Retrying in 3 seconds. Attempt {Attempt}/{MaxAttempts}.",
                    attempt,
                    maxAttempts);

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

        await dbContext.Database.MigrateAsync();
    }
}
