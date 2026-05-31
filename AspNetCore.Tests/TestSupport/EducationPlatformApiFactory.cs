using Common.Models;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace EducationPlatformTests.TestSupport;

public sealed class EducationPlatformApiFactory(bool useDevelopmentAuth = true)
    : WebApplicationFactory<Program>
{
    private readonly string databaseName = Guid.NewGuid().ToString();
    private readonly string dataProtectionPath = Path.Combine(
        TestContext.CurrentContext.WorkDirectory,
        "data-protection",
        Guid.NewGuid().ToString());
    private readonly ServiceProvider databaseProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    public EducationPlatformApiFactory()
        : this(useDevelopmentAuth: true)
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable(
            "Authentication__UseDevelopmentAuth",
            useDevelopmentAuth.ToString());
        Environment.SetEnvironmentVariable("Database__ApplyMigrations", "false");

        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authentication:UseDevelopmentAuth"] = useDevelopmentAuth.ToString(),
                ["Database:ApplyMigrations"] = "false",
            });
        });
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<EducationDbContext>>();
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath));
            services.AddLogging(logging => logging.ClearProviders());
            services.AddDbContext<EducationDbContext>(options =>
                options
                    .UseInMemoryDatabase(databaseName)
                    .UseInternalServiceProvider(databaseProvider));
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await databaseProvider.DisposeAsync();
    }

    public async Task<Guid> SeedCourse(Guid creatorId, decimal price)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EducationDbContext>();
        var timestamp = DateTimeOffset.UtcNow;
        var course = new CourseEntity
        {
            Id = Guid.NewGuid(),
            CreatorId = creatorId,
            Name = "Seeded Course",
            Summary = "Seeded course summary.",
            Description = "Seeded course description.",
            Language = "en",
            Status = PublicationStatus.Published,
            Price = price,
            Tags = ["seeded"],
            CreateTimestamp = timestamp,
            UpdateTimestamp = timestamp,
        };

        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        return course.Id;
    }
}
