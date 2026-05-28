using Application.Modules.Courses.Models;
using Application.Modules.Courses.Requests;
using Data;
using Data.Entities;
using MediatR;

namespace Application.Modules.Courses.Handlers;

internal sealed class AddCourseHandler(EducationDbContext dbContext)
    : IRequestHandler<AddCourseCommand, AddCourseResponse>
{
    public async Task<AddCourseResponse> Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        var timestamp = DateTimeOffset.UtcNow;
        var entity = new CourseEntity
        {
            Id = Guid.NewGuid(),
            CreatorId = request.CreatorId,
            Name = request.Name,
            Summary = request.Summary,
            Description = request.Description,
            Language = request.Language,
            Status = request.Status,
            Price = request.Price,
            Tags = request.Tags.ToArray(),
            CreateTimestamp = timestamp,
            UpdateTimestamp = timestamp,
        };

        dbContext.Courses.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddCourseResponse(entity.Id);
    }
}
