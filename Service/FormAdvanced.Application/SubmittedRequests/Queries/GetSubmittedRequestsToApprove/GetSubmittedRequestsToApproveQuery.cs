using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Queries.GetSubmittedRequestsToApprove
{
    public sealed record GetSubmittedRequestsToApproveQuery(string Approver) : IQuery<List<Domain.Entities.SubmittedRequest>>;
}
