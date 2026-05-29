using Application.Modules.Subscriptions.Models;
using Application.Modules.Subscriptions.Requests;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Subscriptions.Handlers;

internal sealed class GetSubscriptionsHandler(EducationDbContext dbContext)
    : IRequestHandler<GetSubscriptionsRequest, IReadOnlyCollection<Subscription>>
{
    public async Task<IReadOnlyCollection<Subscription>> Handle(
        GetSubscriptionsRequest request,
        CancellationToken cancellationToken)
    {
        return await dbContext.Subscriptions
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.Timespan)
            .Select(x => new Subscription
            {
                Id = x.Id,
                UserId = x.UserId,
                CourseId = x.CourseId,
                Timespan = x.Timespan,
                IsFree = x.IsFree,
                Amount = x.Amount,
                Status = x.Status,
            })
            .ToListAsync(cancellationToken);
    }
}
