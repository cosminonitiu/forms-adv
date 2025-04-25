using FormAdvanced.Application.Common.Caching;
using FormAdvanced.Domain.Entities;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace FormAdvanced.Application.FormRequests.Queries.GetFormRequest
{
    public class GetFormRequestQueryHandler : IRequestHandler<GetFormRequestQuery, CachedResponse<Domain.Entities.FormRequest>>
    {
        private readonly FormRequestCachingService _cachingService;

        public GetFormRequestQueryHandler(IFormRequestService formRequestService, IMemoryCache cache)
        {
            _cachingService = new FormRequestCachingService(
                formRequestService,
                cache,
                TimeSpan.FromHours(1) // Cache duration of 1 hour
            );
        }

        public async Task<CachedResponse<Domain.Entities.FormRequest>> Handle(GetFormRequestQuery request, CancellationToken cancellationToken)
        {
            return await _cachingService.GetByIdAsync(request.Id, request.Owner);
        }
    }
}
