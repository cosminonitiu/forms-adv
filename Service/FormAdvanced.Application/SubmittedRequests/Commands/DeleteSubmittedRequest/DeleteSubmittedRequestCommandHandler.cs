using FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.DeleteSubmittedRequest
{
    public sealed class DeleteSubmittedRequestCommandHandler : ICommandHandler<DeleteSubmittedRequestCommand, Unit>
    {
        private readonly ISubmittedRequestService _formRequestService;

        public DeleteSubmittedRequestCommandHandler(ISubmittedRequestService formRequestService)
        {
            _formRequestService = formRequestService;
        }

        public async Task<Unit> Handle(DeleteSubmittedRequestCommand request, CancellationToken cancellationToken)
        {
            await _formRequestService.DeleteAsync(request.Id, request.Owner);
            return Unit.Value;

        }
    }
}
