using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Exceptions;
using FormAdvanced.Domain.Interfaces;
using MediatR;

namespace FormAdvanced.Application.Notifications.Commands.DeleteNotification
{
    public sealed class DeleteNotificationCommandHandler : ICommandHandler<DeleteNotificationCommand, Unit>
    {
        private readonly INotificationsService _notificationService;

        public DeleteNotificationCommandHandler(INotificationsService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notif = await _notificationService.GetByIdAsync(request.Id, request.Owner);
            if (notif == null)
            {
                throw new NotificationNotFoundException(request.Id);
            }
            await _notificationService.DeleteAsync(request.Id, request.Owner);
            return Unit.Value;
        }
    }
}
