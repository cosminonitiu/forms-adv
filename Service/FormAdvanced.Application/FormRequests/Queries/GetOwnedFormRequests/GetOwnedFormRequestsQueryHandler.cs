using FormAdvanced.Application.Common.Caching;
using FormAdvanced.Domain.Entities;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace FormAdvanced.Application.FormRequests.Queries.GetOwnedFormRequests
{
    public class GetOwnedFormRequestsQueryHandler : IRequestHandler<GetOwnedFormRequestsQuery, CachedResponse<List<Domain.Entities.FormRequest>>>
    {
        private readonly FormRequestCachingService _cachingService;

        public GetOwnedFormRequestsQueryHandler(IFormRequestService formRequestService, IMemoryCache cache)
        {
            _cachingService = new FormRequestCachingService(
                formRequestService,
                cache,
                TimeSpan.FromHours(1) // Cache duration of 1 hour
            );
        }

        public async Task<CachedResponse<List<Domain.Entities.FormRequest>>> Handle(GetOwnedFormRequestsQuery request, CancellationToken cancellationToken)
        {
            return await _cachingService.GetByOwnerAsync(request.Owner);
        }
    }
}
