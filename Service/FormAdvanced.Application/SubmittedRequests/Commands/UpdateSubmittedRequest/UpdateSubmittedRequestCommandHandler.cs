using FormAdvanced.Application.FormRequest.Commands.UpdateFormRequest;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Exceptions;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.UpdateSubmittedRequest
{
    public sealed class UpdateSubmittedRequestCommandHandler : ICommandHandler<UpdateSubmittedRequestCommand, Unit>
    {
        private readonly ISubmittedRequestService _submittedRequestService;

        public UpdateSubmittedRequestCommandHandler(ISubmittedRequestService formRequestService)
        {
            _submittedRequestService = formRequestService;
        }

        public async Task<Unit> Handle(UpdateSubmittedRequestCommand request, CancellationToken cancellationToken)
        {
            var form = await _submittedRequestService.GetByIdAsync(request.Id, request.Owner);
            if (form == null)
            {
                throw new SubmittedRequestNotFoundException(id: request.Id);
            }
            var formUpdate = new Domain.Entities.SubmittedRequest(
                id: request.Id,
                FormName: form.FormName,
                FormId: form.FormId,
                ApproverName: form.ApproverName,
                ApproverUID: form.ApproverUID,
                Created: form.Created,
                Owner: form.Owner,
                OwnerName: form.OwnerName,
                State: form.State,
                Icon: form.Icon,
                Description: form.Description,
                Color: form.Color,
                HideSections: form.HideSections,
                Sections: request.Sections
            );

            await _submittedRequestService.UpsertAsync(formUpdate);

            return Unit.Value;

        }
    }
}
