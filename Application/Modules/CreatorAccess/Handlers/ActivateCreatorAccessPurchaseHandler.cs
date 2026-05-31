using Application.Modules.CreatorAccess.Models;
using Application.Modules.CreatorAccess.Requests;
using Common.Models;
using Data;
using Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.CreatorAccess.Handlers;

internal sealed class ActivateCreatorAccessPurchaseHandler(EducationDbContext dbContext)
    : IRequestHandler<ActivateCreatorAccessPurchaseCommand, ActivateCreatorAccessPurchaseResult>
{
    public async Task<ActivateCreatorAccessPurchaseResult> Handle(
        ActivateCreatorAccessPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.CreatorAccessPurchases
            .FirstOrDefaultAsync(x => x.Id == request.PurchaseId, cancellationToken);

        if (entity is null)
            return new ActivateCreatorAccessPurchaseResult(ActivateCreatorAccessPurchaseStatus.PurchaseNotFound);

        if (entity.Status == CreatorAccessStatus.Active)
            return new ActivateCreatorAccessPurchaseResult(ActivateCreatorAccessPurchaseStatus.AlreadyActive, Map(entity));

        entity.Status = CreatorAccessStatus.Active;

        var creatorExists = await dbContext.Creators
            .AnyAsync(x => x.Id == entity.UserId, cancellationToken);

        if (!creatorExists)
        {
            dbContext.Creators.Add(new CreatorEntity
            {
                Id = entity.UserId,
                Bio = "Creator access activated by simulated payment.",
                Experience = 0,
                AreasOfExpertise = [],
                Languages = [],
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new ActivateCreatorAccessPurchaseResult(ActivateCreatorAccessPurchaseStatus.Activated, Map(entity));
    }

    private static CreatorAccessPurchase Map(CreatorAccessPurchaseEntity entity)
    {
        return new CreatorAccessPurchase
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Timespan = entity.Timespan,
            Amount = entity.Amount,
            Status = entity.Status,
        };
    }
}
