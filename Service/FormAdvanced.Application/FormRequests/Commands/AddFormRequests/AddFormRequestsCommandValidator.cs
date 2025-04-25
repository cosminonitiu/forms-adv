using FluentValidation;

namespace FormAdvanced.Application.FormRequest.Commands.AddFormRequest
{
    public sealed class AddFormRequestsCommandValidator : AbstractValidator<AddFormRequestsCommand>
    {
        public AddFormRequestsCommandValidator()
        {
            RuleFor(x => x.Owner).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Color).NotEmpty();

        }
    }
}
