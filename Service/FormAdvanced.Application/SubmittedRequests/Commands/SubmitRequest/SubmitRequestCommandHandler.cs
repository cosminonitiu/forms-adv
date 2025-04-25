using FormAdvanced.Application.SubmittedRequests.Commands.UpdateSubmittedRequest;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Exceptions;
using FormAdvanced.Domain.Interfaces;
using FormAdvanced.Infrastructure.Services.AzureAD;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.SubmitRequest
{
    public sealed class SubmitRequestCommandHandler : ICommandHandler<SubmitRequestCommand, Unit>
    {
        private readonly ISubmittedRequestService _submittedRequestService;
        private readonly IAzureADService _azureADService;
        private readonly INotificationsService _notificationsService;

        public SubmitRequestCommandHandler(
            ISubmittedRequestService formRequestService, 
            IAzureADService azureADService,
            INotificationsService notificationsService)
        {
            _submittedRequestService = formRequestService;
            _azureADService = azureADService;
            _notificationsService = notificationsService;
        }

        public async Task<Unit> Handle(SubmitRequestCommand request, CancellationToken cancellationToken)
        {
            var manager = await _azureADService.GetUserManagerAsync(request.Owner);
            if (manager == null || manager.Id == null || manager.DisplayName == null)
            {
                throw new UserManagerNotFoundException();
            }
            var form = await _submittedRequestService.GetByIdAsync(request.Id, request.Owner);
            if (form == null)
            {
                throw new SubmittedRequestNotFoundException(id: request.Id);
            }
            var formUpdate = new Domain.Entities.SubmittedRequest(
                id: request.Id,
                FormName: form.FormName,
                FormId: form.FormId,
                ApproverUID: manager.Id,
                ApproverName: manager.DisplayName,
                Created: form.Created,
                Owner: form.Owner,
                OwnerName: form.OwnerName,
                State: "Submitted",
                Icon: form.Icon,
                Description: form.Description,
                Color: form.Color,
                HideSections: form.HideSections,
                Sections: form.Sections
            );

            await _submittedRequestService.UpsertAsync(formUpdate);

            var newNotification = new Domain.Entities.Notification(
                id: new Guid().ToString(),
                Owner: manager.Id,
                Type: "approve",
                FormRequestName: form.FormName,
                SubmittedRequestId: formUpdate.id,
                RequesterUID: form.Owner,
                RequesterName: form.OwnerName
                );

            await _notificationsService.UpsertAsync(newNotification);

            return Unit.Value;

        }
    }
}
