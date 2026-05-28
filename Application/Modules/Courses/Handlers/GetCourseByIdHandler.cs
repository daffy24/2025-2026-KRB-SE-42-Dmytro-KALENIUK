using Application.Modules.Courses.Models;
using Application.Modules.Courses.Requests;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.Handlers;

internal sealed class GetCourseByIdHandler(EducationDbContext dbContext)
    : IRequestHandler<GetCourseByIdRequest, Course?>
{
    public async Task<Course?> Handle(GetCourseByIdRequest request, CancellationToken cancellationToken)
    {
        return await dbContext.Courses
            .AsNoTracking()
            .Where(x => x.Id == request.CourseId)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}
