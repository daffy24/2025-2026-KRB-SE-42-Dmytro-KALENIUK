using Application.Modules.Courses.Models;
using Application.Modules.Courses.Requests;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.Handlers;

internal sealed class UpdateCourseHandler(EducationDbContext dbContext)
    : IRequestHandler<UpdateCourseCommand, Course?>
{
    public async Task<Course?> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var query = dbContext.Courses.Where(x => x.Id == request.CourseId);

        if (!request.CanManageAllCourses)
            query = query.Where(x => x.CreatorId == request.UserId);

        var entity = await query.FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            return null;

        entity.Name = request.Name;
        entity.Summary = request.Summary;
        entity.Description = request.Description;
        entity.Language = request.Language;
        entity.Price = request.Price;
        entity.Tags = request.Tags.ToArray();
        entity.Status = request.Status;
        entity.UpdateTimestamp = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Course
        {
            Id = entity.Id,
            CreatorId = entity.CreatorId,
            Name = entity.Name,
            Summary = entity.Summary,
            Description = entity.Description,
            Language = entity.Language,
            Price = entity.Price,
            Tags = entity.Tags,
            Status = entity.Status,
            UpdateTimestamp = entity.UpdateTimestamp,
        };
    }
}
