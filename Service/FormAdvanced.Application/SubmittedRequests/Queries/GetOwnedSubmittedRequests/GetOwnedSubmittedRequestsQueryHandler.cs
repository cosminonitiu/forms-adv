using FormAdvanced.Application.FormRequests.Queries.GetOwnedFormRequests;
using FormAdvanced.Domain.Interfaces;
using FormAdvanced.Infrastructure.Services.AzureCosmosDB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Queries.GetOwnedSubmittedRequests
{
    public class GetOwnedSubmittedRequestsQueryHandler : IRequestHandler<GetOwnedSubmittedRequestsQuery, List<Domain.Entities.SubmittedRequest>>
    {
        private readonly ISubmittedRequestService _formRequestService;
        public GetOwnedSubmittedRequestsQueryHandler(ISubmittedRequestService formRequestService)
        {
            _formRequestService = formRequestService;
        }

        public async Task<List<Domain.Entities.SubmittedRequest>> Handle(GetOwnedSubmittedRequestsQuery request, CancellationToken cancellationToken)
        {
            var formRequests = await _formRequestService.GetByOwnerAsync(request.Owner);
            return formRequests;
        }
    }
}
