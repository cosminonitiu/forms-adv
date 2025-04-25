using FluentValidation;

namespace FormAdvanced.Application.FormRequest.Commands.DeleteFormRequest
{
    public sealed class DeleteFormRequestsCommandValidator : AbstractValidator<DeleteFormRequestsCommand>
    {
        public DeleteFormRequestsCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
        }
    }
}
