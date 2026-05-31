using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using Data;
using Data.Entities;
using MediatR;

namespace Application.Modules.Lessons.Handlers;

internal sealed class AddLessonHandler(EducationDbContext dbContext)
    : IRequestHandler<AddLessonCommand, AddLessonResponse>
{
    public async Task<AddLessonResponse> Handle(AddLessonCommand request, CancellationToken cancellationToken)
    {
        var timestamp = DateTimeOffset.UtcNow;
        var entity = new LessonEntity
        {
            Id = Guid.NewGuid(),
            CourseId = request.CourseId,
            CreatorId = request.CreatorId,
            Name = request.Name,
            Summary = request.Summary,
            Description = request.Description,
            Text = request.Text,
            Status = request.Status,
            Duration = request.Duration,
            MediaType = request.MediaType,
            CreateTimestamp = timestamp,
            UpdateTimestamp = timestamp,
        };

        dbContext.Lessons.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddLessonResponse(entity.Id);
    }
}
