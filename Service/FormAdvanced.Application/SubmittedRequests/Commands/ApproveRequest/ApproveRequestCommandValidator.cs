using FluentValidation;
using FormAdvanced.Application.SubmittedRequests.Commands.SubmitRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.ApproveRequest
{
    public sealed class ApproveRequestCommandValidator : AbstractValidator<ApproveRequestCommand>
    {
        public ApproveRequestCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
            RuleFor(x => x.Approver).NotEmpty();
        }
    }
}
