using Application.Modules.Lessons.Requests;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Lessons.Handlers;

internal sealed class DeleteLessonHandler(EducationDbContext dbContext)
    : IRequestHandler<DeleteLessonRequest, bool>
{
    public async Task<bool> Handle(DeleteLessonRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Lessons
            .FirstOrDefaultAsync(x => x.Id == request.LessonId && x.CreatorId == request.CreatorId, cancellationToken);

        if (entity is null)
            return false;

        dbContext.Lessons.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
