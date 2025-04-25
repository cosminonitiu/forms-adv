using FormAdvanced.Application.Common.Caching;
using FormAdvanced.BuildingBlocks.Application.Configuration.Queries;
using FormAdvanced.Domain.Entities;

namespace FormAdvanced.Application.FormRequests.Queries.GetFormRequest
{
    public sealed record GetFormRequestQuery(string Id, string Owner) : IQuery<CachedResponse<Domain.Entities.FormRequest>>;
}
