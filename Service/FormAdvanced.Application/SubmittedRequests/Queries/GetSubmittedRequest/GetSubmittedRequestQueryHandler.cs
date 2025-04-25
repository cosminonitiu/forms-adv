using FormAdvanced.Application.SubmittedRequests.Queries.GetOwnedSubmittedRequests;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Queries.GetSubmittedRequest
{
    public class GetSubmittedRequestQueryHandler : IRequestHandler<GetSubmittedRequestQuery,Domain.Entities.SubmittedRequest>
    {
        private readonly ISubmittedRequestService _formRequestService;
        public GetSubmittedRequestQueryHandler(ISubmittedRequestService formRequestService)
        {
            _formRequestService = formRequestService;
        }

        public async Task<Domain.Entities.SubmittedRequest> Handle(GetSubmittedRequestQuery request, CancellationToken cancellationToken)
        {
            var formRequests = await _formRequestService.GetByIdAsync(request.Id, request.Owner);
            return formRequests;
        }
    }
}
