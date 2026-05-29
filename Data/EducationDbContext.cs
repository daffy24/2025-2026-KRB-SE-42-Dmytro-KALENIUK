using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

/// <summary>
/// Represents database context.
/// </summary>
public class EducationDbContext(DbContextOptions<EducationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// The data set to be used to operate with <see cref="CourseEntity"/> entities.
    /// </summary>
    public DbSet<CourseEntity> Courses { get; set; }

    /// <summary>
    /// The data set to be used to operate with <see cref="LessonEntity"/> entities.
    /// </summary>
    public DbSet<LessonEntity> Lessons { get; set; }

    /// <summary>
    /// The data set to be used to operate with <see cref="SubscriptionEntity"/> entities.
    /// </summary>
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }

    /// <summary>
    /// The data set to be used to operate with <see cref="CreatorEntity"/> entities.
    /// </summary>
    public DbSet<CreatorEntity> Creators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EducationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
