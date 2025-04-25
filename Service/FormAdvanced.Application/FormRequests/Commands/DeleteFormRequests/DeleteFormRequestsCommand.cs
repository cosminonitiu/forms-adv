using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using MediatR;

namespace FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest
{
    public sealed record DeleteFormRequestsCommand(string Id, string Owner) : ICommand<Unit>;
}
