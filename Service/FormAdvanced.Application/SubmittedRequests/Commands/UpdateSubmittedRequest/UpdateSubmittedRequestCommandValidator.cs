using FluentValidation;
using FormAdvanced.Application.SubmittedRequests.Commands.UpdateSubmittedRequest;

namespace FormAdvanced.Application.SubmittedRequests.Commands.UpdateSubmittedRequest
{
    public sealed class UpdateSubmittedRequestCommandValidator : AbstractValidator<UpdateSubmittedRequestCommand>
    {
        public UpdateSubmittedRequestCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
        }
    }
}
