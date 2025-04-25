namespace FormAdvanced.Application.FormRequest.Commands.AddFormRequest
{
    public sealed record AddFormRequestsRequest(string Name, string Icon, string Description, string Color, bool HideSections);
}
