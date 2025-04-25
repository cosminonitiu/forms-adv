using FormAdvanced.Domain.Entities;

namespace FormAdvanced.Application.FormRequest.Commands.UpdateFormRequest
{
    public sealed record UpdateFormRequestsRequest(FormSection[] Sections);
}
