using FluentValidation;
using FormAdvanced.Application.SubmittedRequests.Commands.ApproveRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Application.SubmittedRequests.Commands.RejectRequest
{
    public sealed class RejectRequestCommandValidator : AbstractValidator<RejectRequestCommand>
    {
        public RejectRequestCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
            RuleFor(x => x.Approver).NotEmpty();
        }
    }
}
