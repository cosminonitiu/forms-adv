using FormAdvanced.BuildingBlocks.Application.Configuration.Commands;

namespace FormAdvanced.Application.FormRequest.Commands.AddFormRequest
{
    public sealed record AddFormRequestsCommand(string Owner, string Name, string Icon, string Description, string Color, bool HideSections) : ICommand<string>;
}
