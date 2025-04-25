using FormAdvanced.Application.Common.Caching;
using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using FormAdvanced.Domain.Entities;

namespace FormAdvanced.Application.FormRequests.Queries.GetOwnedFormRequests
{
    public sealed record GetOwnedFormRequestsQuery(string Owner) : IQuery<CachedResponse<List<Domain.Entities.FormRequest>>>;
}
