using FormAdvanced.Application.SubmittedRequests.Queries.GetOwnedSubmittedRequests;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Queries.GetSubmittedRequestsToApprove
{
    public class GetSubmittedRequestsToApproveQueryHandler : IRequestHandler<GetSubmittedRequestsToApproveQuery, List<Domain.Entities.SubmittedRequest>>
    {
        private readonly ISubmittedRequestService _formRequestService;
        public GetSubmittedRequestsToApproveQueryHandler(ISubmittedRequestService formRequestService)
        {
            _formRequestService = formRequestService;
        }

        public async Task<List<Domain.Entities.SubmittedRequest>> Handle(GetSubmittedRequestsToApproveQuery request, CancellationToken cancellationToken)
        {
            var formRequests = await _formRequestService.GetByApproverAsync(request.Approver);
            var formsToReturn = new List<Domain.Entities.SubmittedRequest>();
            foreach (var item in formRequests)
            {   
                if(item.State == "Submitted")
                {
                    formsToReturn.Add(item);
                }
            }
            return formsToReturn;
        }
    }
}
