using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Queries.GetOwnedSubmittedRequests
{
    public sealed record GetOwnedSubmittedRequestsQuery(string Owner) : IQuery<List<Domain.Entities.SubmittedRequest>>;
}
