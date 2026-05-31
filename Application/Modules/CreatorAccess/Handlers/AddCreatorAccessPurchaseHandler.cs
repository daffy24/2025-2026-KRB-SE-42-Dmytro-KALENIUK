using Application.Modules.CreatorAccess.Models;
using Application.Modules.CreatorAccess.Requests;
using Common.Models;
using Data;
using Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.CreatorAccess.Handlers;

internal sealed class AddCreatorAccessPurchaseHandler(EducationDbContext dbContext)
    : IRequestHandler<AddCreatorAccessPurchaseCommand, AddCreatorAccessPurchaseResult>
{
    private const decimal CreatorAccessPrice = 100;

    public async Task<AddCreatorAccessPurchaseResult> Handle(
        AddCreatorAccessPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var alreadyCreator = await dbContext.Creators
            .AnyAsync(x => x.Id == request.UserId, cancellationToken);

        if (alreadyCreator)
            return new AddCreatorAccessPurchaseResult(AddCreatorAccessPurchaseStatus.AlreadyCreator);

        var alreadyPurchased = await dbContext.CreatorAccessPurchases
            .AnyAsync(
                x => x.UserId == request.UserId &&
                     x.Status != CreatorAccessStatus.Active,
                cancellationToken);

        if (alreadyPurchased)
            return new AddCreatorAccessPurchaseResult(AddCreatorAccessPurchaseStatus.AlreadyPurchased);

        var entity = new CreatorAccessPurchaseEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Timespan = DateTimeOffset.UtcNow,
            Amount = CreatorAccessPrice,
            Status = CreatorAccessStatus.Pending,
        };

        dbContext.CreatorAccessPurchases.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddCreatorAccessPurchaseResult(AddCreatorAccessPurchaseStatus.Created, Map(entity));
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
