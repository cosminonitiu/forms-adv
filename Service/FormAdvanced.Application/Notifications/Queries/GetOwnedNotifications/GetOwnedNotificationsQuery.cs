using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using FormAdvanced.Domain.Entities;

namespace FormAdvanced.Application.Notifications.Queries.GetOwnedNotifications
{
    public sealed record GetOwnedNotificationsQuery(string Owner) : IQuery<List<Notification>>;
}
