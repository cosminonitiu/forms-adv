using FormAdvanced.Application.FormRequest.Commands.AddFormRequest;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Exceptions;
using FormAdvanced.Domain.Interfaces;
using FormAdvanced.Infrastructure.Services.AzureAD;

namespace FormAdvanced.Application.SubmittedRequests.Commands.SaveDraftRequest
{
    public sealed class SaveDraftRequestCommandHandler : ICommandHandler<SaveDraftRequestCommand, string>
    {
        private readonly ISubmittedRequestService _submittedRequestService;
        private readonly IFormRequestService _formRequestService;
       

        public SaveDraftRequestCommandHandler(
            ISubmittedRequestService submittedRequestService, 
            IFormRequestService formRequestService
            )
        {
            _submittedRequestService = submittedRequestService;
            _formRequestService = formRequestService;      
        }

        public async Task<string> Handle(SaveDraftRequestCommand request, CancellationToken cancellationToken)
        {
            
            var form = await _formRequestService.GetByIdAsync(request.FormId, request.FormOwnerId);
            if(form == null)
            {
                throw new FormRequestNotFoundException(request.FormId);
            }
            var newRequest = new Domain.Entities.SubmittedRequest(
                id: Guid.NewGuid().ToString(),
                Created: DateTime.UtcNow,
                State: "Draft",
                Owner: request.Owner,
                OwnerName: request.OwnerName,
                FormId: request.FormId,
                FormName: form.Name,
                ApproverUID: "",
                ApproverName: "",
                Icon: form.Icon,
                Description: form.Description,
                Color: form.Color,
                HideSections: form.HideSections,
                Sections: request.Sections
            );
            await _submittedRequestService.UpsertAsync(newRequest);
            return newRequest.id;

        }
    }
}
