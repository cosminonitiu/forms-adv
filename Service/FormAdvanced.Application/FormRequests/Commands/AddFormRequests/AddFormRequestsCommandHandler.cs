using FormAdvanced.Application.Common.Caching;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FormAdvanced.Application.FormRequest.Commands.AddFormRequest
{
    public sealed class AddFormRequestsCommandHandler : ICommandHandler<AddFormRequestsCommand, string>
    {
        private readonly FormRequestCachingService _cachingService;

        public AddFormRequestsCommandHandler(IFormRequestService formRequestService, IMemoryCache cache)
        {
            _cachingService = new FormRequestCachingService(
                formRequestService,
                cache,
                TimeSpan.FromHours(1) // Cache duration of 1 hour
            );
        }

        public async Task<string> Handle(AddFormRequestsCommand request, CancellationToken cancellationToken)
        {
            var newRequest = new Domain.Entities.FormRequest(
                id: Guid.NewGuid().ToString(),
                Created: DateTime.UtcNow,
                Owner: request.Owner,
                Name: request.Name,
                Icon: request.Icon,
                Description: request.Description,
                Color: request.Color,
                HideSections: request.HideSections,
                Sections: []
            );
            
            var result = await _cachingService.UpsertAsync(newRequest);
            return newRequest.id;
        }
    }
}