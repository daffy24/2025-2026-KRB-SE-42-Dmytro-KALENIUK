using Application.Modules.Courses.Requests;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.Handlers;

internal sealed class DeleteCourseHandler(EducationDbContext dbContext)
    : IRequestHandler<DeleteCourseRequest, bool>
{
    public async Task<bool> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Courses
            .FirstOrDefaultAsync(x => x.Id == request.CourseId && x.CreatorId == request.CreatorId, cancellationToken);

        if (entity is null)
            return false;

        dbContext.Courses.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
