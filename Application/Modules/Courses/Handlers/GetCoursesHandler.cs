using Application.Modules.Courses.Models;
using Application.Modules.Courses.Requests;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.Handlers;

internal sealed class GetCoursesHandler(EducationDbContext dbContext)
    : IRequestHandler<GetCoursesRequest, IReadOnlyCollection<Course>>
{
    public async Task<IReadOnlyCollection<Course>> Handle(GetCoursesRequest request, CancellationToken cancellationToken)
    {
        return await dbContext.Courses
            .AsNoTracking()
            .Select(x => new Course
            {
                Id = x.Id,
                CreatorId = x.CreatorId,
                Name = x.Name,
                Summary = x.Summary,
                Description = x.Description,
                Language = x.Language,
                Price = x.Price,
                Tags = x.Tags,
                Status = x.Status,
                UpdateTimestamp = x.UpdateTimestamp,
            })
            .ToListAsync(cancellationToken);
    }
}
