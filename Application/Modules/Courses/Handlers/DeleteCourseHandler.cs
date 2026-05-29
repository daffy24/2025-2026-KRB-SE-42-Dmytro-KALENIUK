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
        var query = dbContext.Courses.Where(x => x.Id == request.CourseId);

        if (!request.CanManageAllCourses)
            query = query.Where(x => x.CreatorId == request.UserId);

        var entity = await query.FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            return false;

        dbContext.Courses.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
