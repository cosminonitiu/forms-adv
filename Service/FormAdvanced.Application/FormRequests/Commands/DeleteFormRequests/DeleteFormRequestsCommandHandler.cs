using FormAdvanced.Application.Common.Caching;
using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Exceptions;
using FormAdvanced.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest
{
    public sealed class DeleteFormRequestsCommandHandler : ICommandHandler<DeleteFormRequestsCommand, Unit>
    {
        private readonly FormRequestCachingService _cachingService;

        public DeleteFormRequestsCommandHandler(IFormRequestService formRequestService, IMemoryCache cache)
        {
            _cachingService = new FormRequestCachingService(
                formRequestService,
                cache,
                TimeSpan.FromHours(1) // Cache duration of 1 hour
            );
        }

        public async Task<Unit> Handle(DeleteFormRequestsCommand request, CancellationToken cancellationToken)
        {
            var r = await _cachingService.GetByIdAsync(request.Id, request.Owner); 
            if(r == null)
            {
                throw new FormRequestNotFoundException(request.Id);
            }
            await _cachingService.DeleteAsync(request.Id, request.Owner);
            return Unit.Value;
        }
    }
}