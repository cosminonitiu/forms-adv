using FluentValidation;

namespace FormAdvanced.Application.FormRequest.Commands.UpdateFormRequest
{
    public sealed class UpdateFormRequestsCommandValidator : AbstractValidator<UpdateFormRequestsCommand>
    {
        public UpdateFormRequestsCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
		}
    }
}
