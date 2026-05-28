using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using Common.Models;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Lessons.Handlers;

internal sealed class GetLessonByIdHandler(EducationDbContext dbContext)
    : IRequestHandler<GetLessonByIdRequest, Lesson?>
{
    public async Task<Lesson?> Handle(GetLessonByIdRequest request, CancellationToken cancellationToken)
    {
        return await dbContext.Lessons
            .AsNoTracking()
            .Where(x => x.Id == request.LessonId)
            .Select(x => new Lesson
            {
                Id = x.Id,
                Name = x.Name,
                Summary = x.Summary,
                Description = x.Description,
                Status = x.Status,
                Duration = x.Duration,
                MediaType = MediaType.None,
                CreateTimestamp = x.CreateTimestamp,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
