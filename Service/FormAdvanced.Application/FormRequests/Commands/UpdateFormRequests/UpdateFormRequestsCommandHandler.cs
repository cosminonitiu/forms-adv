using FormAdvanced.Application.Common.Caching;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Exceptions;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace FormAdvanced.Application.FormRequest.Commands.UpdateFormRequest
{
    public sealed class UpdateFormRequestsCommandHandler : ICommandHandler<UpdateFormRequestsCommand, Unit>
    {
        private readonly FormRequestCachingService _cachingService;

        public UpdateFormRequestsCommandHandler(IFormRequestService formRequestService, IMemoryCache cache)
        {
            _cachingService = new FormRequestCachingService(
                formRequestService,
                cache,
                TimeSpan.FromHours(1) // Cache duration of 1 hour
            );
        }

        public async Task<Unit> Handle(UpdateFormRequestsCommand request, CancellationToken cancellationToken)
        {
            var cachedResponse = await _cachingService.GetByIdAsync(request.Id, request.Owner);
            if (cachedResponse.Data == null)
            {
                throw new FormRequestNotFoundException(id: request.Id);
            }

            var form = cachedResponse.Data;
            var formUpdate = new Domain.Entities.FormRequest(
                id: request.Id,
                Created: form.Created,
                Owner: form.Owner,
                Name: form.Name,
                Icon: form.Icon,
                Description: form.Description,
                Color: form.Color,
                HideSections: form.HideSections,
                Sections: request.Sections
            );

            await _cachingService.UpsertAsync(formUpdate);
            return Unit.Value;
        }
    }
}