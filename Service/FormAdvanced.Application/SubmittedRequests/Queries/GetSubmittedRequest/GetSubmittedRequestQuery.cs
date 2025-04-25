using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Queries.GetSubmittedRequest
{
    public sealed record GetSubmittedRequestQuery(string Id, string Owner) : IQuery<Domain.Entities.SubmittedRequest>;
}
