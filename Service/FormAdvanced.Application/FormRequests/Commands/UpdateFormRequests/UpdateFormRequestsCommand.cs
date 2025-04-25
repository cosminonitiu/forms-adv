using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;
using FormAdvanced.Domain.Entities;
using MediatR;

namespace FormAdvanced.Application.FormRequest.Commands.UpdateFormRequest
{
    public sealed record UpdateFormRequestsCommand(string Id, string Owner, FormSection[] Sections) : ICommand<Unit>;
}
