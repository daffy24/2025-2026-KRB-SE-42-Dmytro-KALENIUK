using Application.Modules.Subscriptions.Models;
using Application.Modules.Subscriptions.Requests;
using Common.Models;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Subscriptions.Handlers;

internal sealed class ActivateSubscriptionHandler(EducationDbContext dbContext)
    : IRequestHandler<ActivateSubscriptionCommand, ActivateSubscriptionResult>
{
    public async Task<ActivateSubscriptionResult> Handle(
        ActivateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == request.SubscriptionId, cancellationToken);

        if (entity is null)
            return new ActivateSubscriptionResult(ActivateSubscriptionStatus.SubscriptionNotFound);

        if (entity.Status == SubscriptionStatus.Active)
            return new ActivateSubscriptionResult(ActivateSubscriptionStatus.AlreadyActive, Map(entity));

        entity.Status = SubscriptionStatus.Active;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ActivateSubscriptionResult(ActivateSubscriptionStatus.Activated, Map(entity));
    }

    private static Subscription Map(Data.Entities.SubscriptionEntity entity)
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
