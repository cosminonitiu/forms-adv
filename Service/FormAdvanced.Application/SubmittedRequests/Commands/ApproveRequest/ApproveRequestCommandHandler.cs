using FormAdvanced.Application.SubmittedRequests.Commands.SubmitRequest;
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

namespace FormAdvanced.Application.SubmittedRequests.Commands.ApproveRequest
{
    public sealed class ApproveRequestCommandHandler : ICommandHandler<ApproveRequestCommand, Unit>
    {
        private readonly ISubmittedRequestService _submittedRequestService;
        private readonly IAzureADService _azureADService;

        public ApproveRequestCommandHandler(ISubmittedRequestService formRequestService, IAzureADService azureADService)
        {
            _submittedRequestService = formRequestService;
            _azureADService = azureADService;
        }

        public async Task<Unit> Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
        {
            var form = await _submittedRequestService.GetByIdAsync(request.Id, request.Owner);
            if (form == null)
            {
                throw new SubmittedRequestNotFoundException(id: request.Id);
            }
            if(form.ApproverUID != request.Approver)
            {
                throw new NotTheApproverException();
            }
            var formUpdate = new Domain.Entities.SubmittedRequest(
                id: request.Id,
                FormName: form.FormName,
                FormId: form.FormId,
                ApproverUID: form.ApproverUID,
                ApproverName: form.ApproverName,
                Created: form.Created,
                Owner: form.Owner,
                OwnerName: form.OwnerName,
                State: "Approved",
                Icon: form.Icon,
                Description: form.Description,
                Color: form.Color,
                HideSections: form.HideSections,
                Sections: form.Sections
            );

            await _submittedRequestService.UpsertAsync(formUpdate);

            return Unit.Value;

        }
    }
}
