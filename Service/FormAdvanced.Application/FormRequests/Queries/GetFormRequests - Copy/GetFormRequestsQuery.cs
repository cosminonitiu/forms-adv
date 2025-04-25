using FormAdvanced.Application.Common.Caching;
using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using FormAdvanced.Domain.Entities;

namespace FormAdvanced.Application.FormRequests.Queries.GetFormRequests
{
    public sealed record GetFormRequestsQuery() : IQuery<CachedResponse<List<Domain.Entities.FormRequest>>>;
}
