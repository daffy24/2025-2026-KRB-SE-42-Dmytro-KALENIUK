using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using Common.Models;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Lessons.Handlers;

internal sealed class GetLessonsHandler(EducationDbContext dbContext)
    : IRequestHandler<GetLessonsRequest, IReadOnlyCollection<Lesson>>
{
    public async Task<IReadOnlyCollection<Lesson>> Handle(GetLessonsRequest request, CancellationToken cancellationToken)
    {
        return await dbContext.Lessons
            .AsNoTracking()
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
            .ToListAsync(cancellationToken);
    }
}
