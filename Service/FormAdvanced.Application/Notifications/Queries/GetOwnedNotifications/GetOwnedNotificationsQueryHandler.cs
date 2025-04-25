using FormAdvanced.Domain.Interfaces;
using MediatR;

namespace FormAdvanced.Application.Notifications.Queries.GetOwnedNotifications
{
    public class GetOwnedNotificationsQueryHandler : IRequestHandler<GetOwnedNotificationsQuery, List<Domain.Entities.Notification>>
    {
        private readonly INotificationsService _notificationService;

        public GetOwnedNotificationsQueryHandler(INotificationsService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<List<Domain.Entities.Notification>> Handle(GetOwnedNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationService.GetByOwnerAsync(request.Owner);
        }
    }
}
