using Application.Modules.Subscriptions.Models;
using Application.Modules.Subscriptions.Requests;
using Common.Models;
using Data;
using Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Subscriptions.Handlers;

internal sealed class AddSubscriptionHandler(EducationDbContext dbContext)
    : IRequestHandler<AddSubscriptionCommand, AddSubscriptionResult>
{
    public async Task<AddSubscriptionResult> Handle(
        AddSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var courseEntity = await dbContext.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

        if (courseEntity is null)
            return new AddSubscriptionResult(AddSubscriptionStatus.CourseNotFound);

        if (courseEntity.Status != PublicationStatus.Published)
            return new AddSubscriptionResult(AddSubscriptionStatus.CourseNotPublished);

        if (courseEntity.CreatorId == request.UserId)
            return new AddSubscriptionResult(AddSubscriptionStatus.OwnCourse);

        var alreadySubscribed = await dbContext.Subscriptions
            .AnyAsync(x => x.UserId == request.UserId && x.CourseId == request.CourseId, cancellationToken);

        if (alreadySubscribed)
            return new AddSubscriptionResult(AddSubscriptionStatus.AlreadySubscribed);

        var isFree = courseEntity.Price == 0;
        var entity = new SubscriptionEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            CourseId = request.CourseId,
            Timespan = DateTimeOffset.UtcNow,
            IsFree = isFree,
            Amount = courseEntity.Price,
            Status = isFree ? SubscriptionStatus.Active : SubscriptionStatus.Pending,
        };

        dbContext.Subscriptions.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddSubscriptionResult(AddSubscriptionStatus.Created, Map(entity));
    }

    private static Subscription Map(SubscriptionEntity entity)
    {
        return new Subscription
        {
            Id = entity.Id,
            UserId = entity.UserId,
            CourseId = entity.CourseId,
            Timespan = entity.Timespan,
            IsFree = entity.IsFree,
            Amount = entity.Amount,
            Status = entity.Status,
        };
    }
}
