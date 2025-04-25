using FluentValidation;
using FormAdvanced.Application.SubmittedRequests.Commands.UpdateSubmittedRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.SubmitRequest
{
    public sealed class SubmitRequestCommandValidator : AbstractValidator<SubmitRequestCommand>
    {
        public SubmitRequestCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
        }
    }
}
